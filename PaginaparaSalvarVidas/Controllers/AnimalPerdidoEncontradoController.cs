using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
using PaginaparaSalvarVidas.Models;
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

        public IActionResult Index()
        {
            var lista = _context.AnimalesPerdidosEncontrados
                .Include(a => a.Animal)
                .Select(a => new AnimalPerdidoEncontradoVM
                {
                    Id = a.Id,
                    NombreAnimal = a.Animal != null ? a.Animal.Nombre : "No registrado",
                    Estado = a.Animal != null ? a.Animal.Estado.ToString() : "Sin estado",
                    Descripcion = a.Descripcion,
                    Direccion = a.Direccion,
                    TelefonoContacto = a.TelefonoContacto,
                    Fecha = a.Fecha
                })
                .ToList();

            return View(lista);
        }

        // GET: AnimalPerdidoEncontrado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesPerdidosEncontrados
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        // GET: AnimalPerdidoEncontrado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnimalPerdidoEncontrado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,AnimalId,Descripcion,Direccion,TelefonoContacto,Fecha")] AnimalPerdidoEncontrado entidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entidad);
        }

        // GET: AnimalPerdidoEncontrado/Edit/5
        public async Task<IActionResult>edit(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesPerdidosEncontrados.FindAsync(id);
            if (entidad == null) return NotFound();

            return View(entidad);
        }

        // POST: AnimalPerdidoEncontrado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,AnimalId,Descripcion,Direccion,TelefonoContacto,Fecha")] AnimalPerdidoEncontrado entidad)
        {
            if (id != entidad.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(entidad.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(entidad);
        }

        // GET: AnimalPerdidoEncontrado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesPerdidosEncontrados
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        // POST: AnimalPerdidoEncontrado/Delete/5
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
    }
}