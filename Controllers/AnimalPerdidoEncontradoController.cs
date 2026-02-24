using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
using PaginaparaSalvarVidas.Models;

namespace PaginaparaSalvarVidas.Controllers
{
    public class AnimalPerdidoEncontradoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalPerdidoEncontradoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1) IDs de animales Perdidos o Encontrados
            var idsPE = await _context.Animales
                .AsNoTracking()
                .Where(a =>
                    a.Estado == EstadoAnimal.Perdido ||
                    a.Estado == EstadoAnimal.Encontrado)
                .Select(a => a.Id)
                .ToListAsync();

            // 2) IDs ya registrados en reportes
            var idsYaRegistrados = await _context.AnimalesPerdidosEncontrados
                .AsNoTracking()
                .Where(r => r.AnimalId.HasValue && idsPE.Contains(r.AnimalId.Value))
                .Select(r => r.AnimalId!.Value)
                .ToListAsync();

            // 3) Crear faltantes
            var faltantes = idsPE.Except(idsYaRegistrados).ToList();

            if (faltantes.Count > 0)
            {
                var nuevos = faltantes.Select(idAnimal => new AnimalPerdidoEncontrado
                {
                    AnimalId = idAnimal,
                    Descripcion = "",
                    Direccion = "",
                    TelefonoContacto = "",
                    Fecha = DateTime.Today
                });

                _context.AnimalesPerdidosEncontrados.AddRange(nuevos);
                await _context.SaveChangesAsync();
            }

            // 4) Construir ViewModel
            var lista = await _context.AnimalesPerdidosEncontrados
                .Include(r => r.Animal)
                .Select(r => new AnimalPerdidoEncontradoVM
                {
                    Id = r.Id,
                    NombreAnimal = r.Animal != null
                        ? r.Animal.Nombre
                        : "No registrado",
                    Estado = r.Animal != null
                        ? r.Animal.Estado.ToString()
                        : "Sin estado",
                    Descripcion = r.Descripcion,
                    Direccion = r.Direccion,
                    TelefonoContacto = r.TelefonoContacto,
                    Fecha = r.Fecha
                })
                .ToListAsync();

            return View(lista);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesPerdidosEncontrados
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        // ✅ GET Create (FIX)
        public IActionResult Create()
        {
            return View(new AnimalPerdidoEncontrado());
        }

        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AnimalId,Descripcion,Direccion,TelefonoContacto,Fecha")] AnimalPerdidoEncontrado entidad)
        {
            // por si tenés navegación Animal = null! en el modelo
            ModelState.Remove("Animal");

            if (!ModelState.IsValid)
                return View(entidad);

            _context.AnimalesPerdidosEncontrados.Add(entidad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesPerdidosEncontrados.FindAsync(id);
            if (entidad == null) return NotFound();

            CargarAnimales(entidad.AnimalId);

            return View(entidad);
        }

        // ✅ POST Update (más seguro)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,AnimalId,Descripcion,Direccion,TelefonoContacto,Fecha")] AnimalPerdidoEncontrado entidad)
        {
            if (id != entidad.Id) return BadRequest();

            ModelState.Remove("Animal");

            if (!ModelState.IsValid)
            {
                CargarAnimales(entidad.AnimalId);
                return View(entidad);
            }

            var entidadBD = await _context.AnimalesPerdidosEncontrados.FindAsync(id);
            if (entidadBD == null) return NotFound();


            entidadBD.AnimalId = entidad.AnimalId;
            entidadBD.Descripcion = entidad.Descripcion;
            entidadBD.Direccion = entidad.Direccion;
            entidadBD.TelefonoContacto = entidad.TelefonoContacto;
            entidadBD.Fecha = entidad.Fecha;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesPerdidosEncontrados
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        // POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entidad = await _context.AnimalesPerdidosEncontrados.FindAsync(id);

            if (entidad != null)
            {
                _context.AnimalesPerdidosEncontrados.Remove(entidad);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool Exists(int id)
        {
            return _context.AnimalesPerdidosEncontrados.Any(e => e.Id == id);
        }

        private void CargarAnimales(int? seleccionado = null)
        {
            ViewBag.Animales = _context.Animales
                .AsNoTracking()
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Nombre,
                    Selected = seleccionado.HasValue && a.Id == seleccionado.Value
                })
                .ToList();
        }
    }
}