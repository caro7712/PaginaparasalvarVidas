using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
using PaginaparaSalvarVidas.Models;

namespace PaginaparaSalvarVidas.Controllers
{
    public class AnimalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Animales
        public async Task<IActionResult> Index()
        {
            return View(await _context.Animales.ToListAsync());
        }

        // GET: Animales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var animal = await _context.Animales
                .FirstOrDefaultAsync(m => m.Id == id);

            if (animal == null)
                return NotFound();

            return View(animal);
        }

        // GET: Animales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Animales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Animal animal)
        {
            if (ModelState.IsValid)
            {
                _context.Animales.Add(animal);
                await _context.SaveChangesAsync();

                // 🔥 Lógica según estado
                switch (animal.Estado)
                {
                    case EstadoAnimal.EnTransito:

                        var transito = new AnimalEnTransito
                        {
                            AnimalId = animal.Id,
                            FechaIngreso = DateTime.Now
                        };

                        _context.AnimalesEnTransito.Add(transito);
                        break;


                    case EstadoAnimal.EnAdopcion:

                        var adopcion = new AnimalEnAdopcion
                        {
                            AnimalId = animal.Id,
                            FechaAdopcion = DateTime.Now
                        };

                        _context.AnimalesEnAdopcion.Add(adopcion);
                        break;


                    case EstadoAnimal.Comunitario:

                        var comunitario = new AnimalComunitario
                        {
                            AnimalId = animal.Id,
                            LugarHabitual = "No especificado",
                            AptoParaAdopcion = false
                        };

                        _context.AnimalesComunitarios.Add(comunitario);
                        break;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(animal);
        }

        // GET: Animales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var animal = await _context.Animales.FindAsync(id);

            if (animal == null)
                return NotFound();

            return View(animal);
        }

        // POST: Animales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Nombre,Tipo,Raza,Edad,Foto,Estado")] Animal animal)
        {
            if (id != animal.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(animal);
        }

        // GET: Animales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var animal = await _context.Animales
                .FirstOrDefaultAsync(m => m.Id == id);

            if (animal == null)
                return NotFound();

            return View(animal);
        }

        // POST: Animales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animales.FindAsync(id);

            if (animal != null)
            {
                _context.Animales.Remove(animal);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id)
        {
            return _context.Animales.Any(e => e.Id == id);
        }
    }
}
