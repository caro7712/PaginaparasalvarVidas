using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
using PaginaparaSalvarVidas.Models;

namespace PaginaparaSalvarVidas.Controllers
{
    public class FamiliaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FamiliaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Familias.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.Familias
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Nombre,Direccion,Telefono")] Familia entidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entidad);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.Familias.FindAsync(id);
            if (entidad == null) return NotFound();

            return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,
            [Bind("Id,Nombre,Direccion,Telefono")] Familia entidad)
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

            var entidad = await _context.Familias
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null) return NotFound();

            return View(entidad);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entidad = await _context.Familias.FindAsync(id);

            if (entidad != null)
            {
                _context.Familias.Remove(entidad);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool Exists(int id)
        {
            return _context.Familias.Any(e => e.Id == id);
        }
    }
}