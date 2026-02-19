using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
using PaginaparaSalvarVidas.Models;
using PaginaparaSalvarVidas.Models;

namespace PaginaparaSalvarVidas.Controllers
{
    public class AnimalComunitarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalComunitarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnimalComunitario
        public async Task<IActionResult> Index()
        {
            var lista = _context.AnimalesComunitarios
                .Include(a => a.Animal);

            return View(await lista.ToListAsync());
        }

        // GET: AnimalComunitario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var entidad = await _context.AnimalesComunitarios
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null)
                return NotFound();

            return View(entidad);
        }

        // GET: AnimalComunitario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnimalComunitario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,AnimalId,LugarHabitual,AptoParaAdopcion")] AnimalComunitario entidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entidad);
        }

        // GET: AnimalComunitario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var entidad = await _context.AnimalesComunitarios.FindAsync(id);

            if (entidad == null)
                return NotFound();

            return View(entidad);
        }

        // POST: AnimalComunitario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,AnimalId,LugarHabitual,AptoParaAdopcion")] AnimalComunitario entidad)
        {
            if (id != entidad.Id)
                return NotFound();

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

        // GET: AnimalComunitario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var entidad = await _context.AnimalesComunitarios
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entidad == null)
                return NotFound();

            return View(entidad);
        }

        // POST: AnimalComunitario/Delete/5
        [HttpPost, ActionName("Delete")]
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