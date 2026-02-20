using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
using PaginaparaSalvarVidas.Models;
using PaginaparaSalvarVidas.Models;

namespace PaginaparaSalvarVidas.Controllers
{
    public class AnimalEnAdopcionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalEnAdopcionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lista = _context.AnimalesEnAdopcion
                .Include(a => a.Animal)
                .Include(a => a.Familia);

            return View(await lista.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesEnAdopcion
                .Include(a => a.Animal)
                .Include(a => a.Familia)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,AnimalId,FamiliaId,FechaAdopcion")] AnimalEnAdopcion entidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entidad);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesEnAdopcion.FindAsync(id);
            if (entidad == null) return NotFound();

            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,AnimalId,FamiliaId,FechaAdopcion")] AnimalEnAdopcion entidad)
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesEnAdopcion
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entidad = await _context.AnimalesEnAdopcion.FindAsync(id);

            if (entidad != null)
            {
                _context.AnimalesEnAdopcion.Remove(entidad);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool Exists(int id)
        {
            return _context.AnimalesEnAdopcion.Any(e => e.Id == id);
        }
    }
}