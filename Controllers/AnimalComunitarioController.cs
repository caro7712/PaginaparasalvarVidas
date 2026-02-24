using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
using PaginaparaSalvarVidas.Models;

namespace PaginaparaSalvarVidas.Controllers
{
    [Authorize]
    public class AnimalComunitarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalComunitarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Combo de animales por NOMBRE (Text) pero guarda AnimalId (Value)
        private void CargarAnimalesCombo(int? animalSeleccionado = null)
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
        }

        // GET: AnimalComunitario
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // 1) IDs de animales que están en estado Comunitario
            var idsComunitarios = await _context.Animales
                .AsNoTracking()
                .Where(a => a.Estado == EstadoAnimal.Comunitario)
                .Select(a => a.Id)
                .ToListAsync();

            // 2) IDs ya existentes en la tabla AnimalesComunitarios
            var idsYaRegistrados = await _context.AnimalesComunitarios
                .AsNoTracking()
                .Where(ac => idsComunitarios.Contains(ac.AnimalId))
                .Select(ac => ac.AnimalId)
                .ToListAsync();

            // 3) Crear registros faltantes
            var faltantes = idsComunitarios.Except(idsYaRegistrados).ToList();

            if (faltantes.Count > 0)
            {
                var nuevos = faltantes.Select(idAnimal => new AnimalComunitario
                {
                    AnimalId = idAnimal,
                    LugarHabitual = "",         // o "Sin especificar"
                    AptoParaAdopcion = false
                });

                _context.AnimalesComunitarios.AddRange(nuevos);
                await _context.SaveChangesAsync();
            }

            // 4) Listar SOLO comunitarios (ya sincronizados)
            var lista = await _context.AnimalesComunitarios
                .Include(ac => ac.Animal)
                .Where(ac => ac.Animal != null && ac.Animal.Estado == EstadoAnimal.Comunitario)
                .ToListAsync();

            return View(lista);
        }

        // GET: AnimalComunitario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesComunitarios
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        // GET: AnimalComunitario/Create
        [Authorize(Roles = "Admin,Voluntario")]
        public IActionResult Create()
        {
            CargarAnimalesCombo();
            return View(new AnimalComunitario()); // ✅ era AnimalEnAdopcion (bug)
        }

        // POST: AnimalComunitario/Create
        [HttpPost]
        [Authorize(Roles = "Admin,Voluntario")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AnimalId,LugarHabitual,AptoParaAdopcion")] AnimalComunitario entidad)
        {
            // si tu modelo tiene navegación Animal = null! esto evita que rompa el POST
            ModelState.Remove("Animal");

            if (!ModelState.IsValid)
            {
                CargarAnimalesCombo(entidad.AnimalId);
                return View(entidad);
            }

            _context.AnimalesComunitarios.Add(entidad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AnimalComunitario/Update/5
        [Authorize(Roles = "Admin,Voluntario")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesComunitarios.FindAsync(id);
            if (entidad == null) return NotFound();

            CargarAnimalesCombo(entidad.AnimalId); // ✅ selecciona el actual
            return View(entidad);
        }

        // POST: AnimalComunitario/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Voluntario")]
        public async Task<IActionResult> Update(int id, [Bind("Id,AnimalId,LugarHabitual,AptoParaAdopcion")] AnimalComunitario entidad)
        {
            if (id != entidad.Id) return BadRequest();

            // evitar "Animal required" si tu nav prop es no-nullable
            ModelState.Remove("Animal");

            if (!ModelState.IsValid)
            {
                CargarAnimalesCombo(entidad.AnimalId);
                return View(entidad);
            }

            // ✅ patrón seguro: traer de DB y actualizar campos
            var entidadBD = await _context.AnimalesComunitarios.FindAsync(id);
            if (entidadBD == null) return NotFound();

            entidadBD.AnimalId = entidad.AnimalId;
            entidadBD.LugarHabitual = entidad.LugarHabitual;
            entidadBD.AptoParaAdopcion = entidad.AptoParaAdopcion;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AnimalComunitario/Delete/5
        [Authorize(Roles = "Admin,Voluntario")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesComunitarios
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        // POST: AnimalComunitario/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Voluntario")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entidad = await _context.AnimalesComunitarios.FindAsync(id);

            if (entidad != null)
            {
                _context.AnimalesComunitarios.Remove(entidad);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool Exists(int id)
        {
            return _context.AnimalesComunitarios.Any(e => e.Id == id);
        }
    }
}