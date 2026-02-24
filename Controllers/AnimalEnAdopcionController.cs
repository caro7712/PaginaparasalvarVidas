using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
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

        // =======================
        // INDEX
        // =======================
        public async Task<IActionResult> Index()
        {
            // 1) IDs de animales en estado EnAdopcion
            var idsEnAdopcion = await _context.Animales
                .AsNoTracking()
                .Where(a => a.Estado == EstadoAnimal.EnAdopcion)
                .Select(a => a.Id)
                .ToListAsync();

            // 2) IDs ya existentes en AnimalEnAdopcion
            var idsYaRegistrados = await _context.AnimalesEnAdopcion
                .AsNoTracking()
                .Where(x => idsEnAdopcion.Contains(x.AnimalId))
                .Select(x => x.AnimalId)
                .ToListAsync();

            // 3) Crear faltantes
            var faltantes = idsEnAdopcion.Except(idsYaRegistrados).ToList();

            if (faltantes.Count > 0)
            {
                var nuevos = faltantes.Select(idAnimal => new AnimalEnAdopcion
                {
                    AnimalId = idAnimal,
                    FamiliaId = 1,                 // ⚠️ ver nota abajo
                    FechaAdopcion = DateTime.Today // placeholder
                });

                _context.AnimalesEnAdopcion.AddRange(nuevos);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    // si entran 2 requests a la vez y el índice único evita duplicados
                }
            }

            // 4) Listar solo los que siguen EnAdopcion
            var lista = await _context.AnimalesEnAdopcion
                .Include(x => x.Animal)
                .Include(x => x.Familia)
                .Where(x => x.Animal != null && x.Animal.Estado == EstadoAnimal.EnAdopcion)
                .ToListAsync();

            return View(lista);
        }

        // =======================
        // CREATE (GET)
        // =======================
        public async Task<IActionResult> Create()
        {
            var vm = new AnimalEnAdopcionVM
            {
                AnimalEnAdopcion = new AnimalEnAdopcion(),

                Animales = await _context.Animales
                    .Where(a => a.Estado == EstadoAnimal.EnAdopcion)
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Nombre
                    })
                    .ToListAsync(),

                Familias = await _context.Familias
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Nombre
                    })
                    .ToListAsync()
            };

            return View(vm);
        }

        // =======================
        // CREATE (POST)
        // =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnimalEnAdopcionVM vm)
        {
            var entidad = vm.AnimalEnAdopcion;

            if (ModelState.IsValid)
            {
                _context.AnimalesEnAdopcion.Add(entidad);

                var animal = await _context.Animales.FindAsync(entidad.AnimalId);
                if (animal != null)
                    animal.Estado = EstadoAnimal.Adoptado;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // 🔁 Recargar combos si hay error
            vm.Animales = await _context.Animales
                .Where(a => a.Estado == EstadoAnimal.EnAdopcion)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Nombre
                })
                .ToListAsync();

            vm.Familias = await _context.Familias
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Nombre
                })
                .ToListAsync();

            return View(vm);
        }

        // =======================
        // DELETE (POST)
        // =======================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entidad = await _context.AnimalesEnAdopcion
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (entidad != null)
            {
                if (entidad.Animal != null)
                    entidad.Animal.Estado = EstadoAnimal.EnAdopcion;

                _context.AnimalesEnAdopcion.Remove(entidad);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // =======================
        // UPDATE (GET)
        // =======================
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var entidad = await _context.AnimalesEnAdopcion
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null) return NotFound();

            var vm = new AnimalEnAdopcionVM
            {
                AnimalEnAdopcion = entidad,

                Animales = await _context.Animales
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Nombre
                    })
                    .ToListAsync(),

                Familias = await _context.Familias
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Nombre // o Apellido (lo que tengas)
                    })
                    .ToListAsync()
            };

            return View(vm);
        }

        // =======================
        // UPDATE (POST)
        // =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, AnimalEnAdopcionVM vm)
        {
            if (vm.AnimalEnAdopcion == null) return BadRequest();
            if (id != vm.AnimalEnAdopcion.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                // recargar combos
                vm.Animales = await _context.Animales
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Nombre
                    })
                    .ToListAsync();

                vm.Familias = await _context.Familias
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Nombre
                    })
                    .ToListAsync();

                return View(vm);
            }

            var entidadBD = await _context.AnimalesEnAdopcion.FindAsync(id);
            if (entidadBD == null) return NotFound();

            entidadBD.AnimalId = vm.AnimalEnAdopcion.AnimalId;
            entidadBD.FamiliaId = vm.AnimalEnAdopcion.FamiliaId;
            entidadBD.FechaAdopcion = vm.AnimalEnAdopcion.FechaAdopcion;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}