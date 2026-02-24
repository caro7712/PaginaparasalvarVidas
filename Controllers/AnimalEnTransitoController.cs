using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
using PaginaparaSalvarVidas.Models;

namespace PaginaparaSalvarVidas.Controllers
{
    public class AnimalEnTransitoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalEnTransitoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Cargar combos para selects (AnimalId / FamiliaId)
        private void CargarCombos(int? animalSeleccionado = null, int? familiaSeleccionada = null)
        {
            ViewBag.Animales = _context.Animales
                .AsNoTracking()
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Nombre,
                    Selected = animalSeleccionado.HasValue && a.Id == animalSeleccionado.Value
                })
                .ToList();

            ViewBag.Familias = _context.Familias
                .AsNoTracking()
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Nombre,
                    Selected = familiaSeleccionada.HasValue && f.Id == familiaSeleccionada.Value
                })
                .ToList();
        }

        private string ObtenerErroresModelState()
        {
            var errores = ModelState
                .Where(kvp => kvp.Value != null && kvp.Value.Errors.Count > 0)
                .Select(kvp =>
                {
                    var msgs = string.Join(" | ", kvp.Value!.Errors.Select(e => e.ErrorMessage));
                    return $"{kvp.Key}: {msgs}";
                });

            return string.Join(" || ", errores);
        }

        public async Task<IActionResult> Index()
{
    // 1) IDs de animales que están en estado EnTransito
    var idsEnTransito = await _context.Animales
        .AsNoTracking()
        .Where(a => a.Estado == EstadoAnimal.EnTransito)
        .Select(a => a.Id)
        .ToListAsync();

    // 2) IDs ya existentes en AnimalesEnTransito
    var idsYaRegistrados = await _context.AnimalesEnTransito
        .AsNoTracking()
        .Where(t => idsEnTransito.Contains(t.AnimalId))
        .Select(t => t.AnimalId)
        .ToListAsync();

    // 3) Crear faltantes
    var faltantes = idsEnTransito.Except(idsYaRegistrados).ToList();

    if (faltantes.Count > 0)
    {
        var nuevos = faltantes.Select(idAnimal => new AnimalEnTransito
        {
            AnimalId = idAnimal,
            FamiliaId = null,                 // ✅ ahora se puede
            FechaIngreso = DateTime.Today,    // placeholder
            FechaSalida = null
        });

        _context.AnimalesEnTransito.AddRange(nuevos);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // si entraron 2 requests y el índice único frenó duplicado, ignoramos
        }
    }

    // 4) Listar SOLO los que siguen EnTransito
    var lista = await _context.AnimalesEnTransito
        .Include(t => t.Animal)
        .Include(t => t.Familia)
        .Where(t => t.Animal != null && t.Animal.Estado == EstadoAnimal.EnTransito)
        .ToListAsync();

    return View(lista);
}

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesEnTransito
                .Include(a => a.Animal)
                .Include(a => a.Familia)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        // GET: AnimalEnTransito/Create
        public IActionResult Create()
        {
            CargarCombos();
            return View(new AnimalEnTransito());
        }

        // POST: AnimalEnTransito/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AnimalId,FamiliaId,FechaIngreso,FechaSalida")] AnimalEnTransito entidad)
        {
            if (!ModelState.IsValid)
            {
                TempData["SwalError"] = ObtenerErroresModelState();
                CargarCombos(entidad.AnimalId, entidad.FamiliaId);
                return View(entidad);
            }

            // (Opcional) bloquear duplicado por AnimalId solo en Create
            bool duplicado = await _context.AnimalesEnTransito
                .AsNoTracking()
                .AnyAsync(x => x.AnimalId == entidad.AnimalId);

            if (duplicado)
            {
                TempData["SwalError"] = "AnimalId: Ese animal ya está registrado en tránsito. No se permiten duplicados.";
                CargarCombos(entidad.AnimalId, entidad.FamiliaId);
                return View(entidad);
            }

            _context.AnimalesEnTransito.Add(entidad);

            try
            {
                await _context.SaveChangesAsync();
                TempData["SwalOk"] = "Se creó correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["SwalError"] = "No se pudo guardar por una restricción de base de datos.";
                CargarCombos(entidad.AnimalId, entidad.FamiliaId);
                return View(entidad);
            }
        }

        // GET: AnimalEnTransito/Update/5
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesEnTransito.FindAsync(id);
            if (entidad == null) return NotFound();

            CargarCombos(entidad.AnimalId, entidad.FamiliaId);
            return View(entidad);
        }

        // POST: AnimalEnTransito/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,AnimalId,FamiliaId,FechaIngreso,FechaSalida")] AnimalEnTransito entidad)
        {
            if (id != entidad.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                // ✅ ahora te dice EXACTAMENTE el campo inválido
                TempData["SwalError"] = ObtenerErroresModelState();
                CargarCombos(entidad.AnimalId, entidad.FamiliaId);
                return View(entidad);
            }

            var entidadBD = await _context.AnimalesEnTransito.FindAsync(id);
            if (entidadBD == null) return NotFound();

            // ✅ Solo validar duplicado si cambió el AnimalId
            if (entidadBD.AnimalId != entidad.AnimalId)
            {
                bool duplicado = await _context.AnimalesEnTransito
                    .AsNoTracking()
                    .AnyAsync(x => x.AnimalId == entidad.AnimalId && x.Id != entidad.Id);

                if (duplicado)
                {
                    TempData["SwalError"] = "AnimalId: Ese animal ya está registrado en tránsito. No se permiten duplicados.";
                    CargarCombos(entidad.AnimalId, entidad.FamiliaId);
                    return View(entidad);
                }
            }

            entidadBD.AnimalId = entidad.AnimalId;
            entidadBD.FamiliaId = entidad.FamiliaId;
            entidadBD.FechaIngreso = entidad.FechaIngreso;
            entidadBD.FechaSalida = entidad.FechaSalida;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SwalOk"] = "Se actualizó correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["SwalError"] = "No se pudo guardar por una restricción de la base de datos (duplicado o FK).";
                CargarCombos(entidad.AnimalId, entidad.FamiliaId);
                return View(entidad);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesEnTransito
                .Include(a => a.Animal)
                .Include(a => a.Familia)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entidad = await _context.AnimalesEnTransito.FindAsync(id);

            if (entidad != null)
            {
                _context.AnimalesEnTransito.Remove(entidad);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}