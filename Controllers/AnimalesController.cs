using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
using PaginaparaSalvarVidas.Models;
using PaginaparaSalvarVidas.Models.Procedimiento_almacenado;

namespace PaginaparaSalvarVidas.Controllers
{
    public class AnimalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnimalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // FILTRO (SP)
        // ==========================
        [HttpGet]
        public async Task<IActionResult> Filtrar(
            EstadoAnimal? estado,
            string? tipo,
            int? edadMin,
            int? edadMax)
        {
            var sp = new SP_FiltrarAnimalesAvanzado(_context);
            var lista = await sp.Ejecutar(estado, tipo, edadMin, edadMax);

            return View("Index", lista);
        }

        // ==========================
        // INDEX (SP)
        // ==========================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sp = new SP_FiltrarAnimalesAvanzado(_context);
            var lista = await sp.Ejecutar();
            return View(lista);
        }

        // ==========================
        // DETAILS
        // ==========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _context.Animales.FirstOrDefaultAsync(a => a.Id == id);
            if (animal == null) return NotFound();

            return View(animal);
        }

        // ==========================
        // CREATE (GET) -> USA VM
        // ==========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new AnimalVM
            {
                Familias = await _context.Familias
                    .OrderBy(f => f.Nombre)
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Nombre
                    })
                    .ToListAsync()
            };

            // defaults opcionales
            vm.Animal.Estado = EstadoAnimal.EnAdopcion;
            vm.AnimalEnTransito.FechaIngreso = DateTime.Today;

            return View(vm);
        }

        // ==========================
        // CREATE (POST) -> GUARDA ANIMAL + SUBTIPO SEGÚN ESTADO
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnimalVM vm)
        {
            // Recargar combos SIEMPRE antes de volver a la vista
            vm.Familias = await _context.Familias
                .OrderBy(f => f.Nombre)
                .Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.Nombre
                })
                .ToListAsync();

            // ✅ Evitar que validaciones de subtipos NO usados rompan el ModelState
            switch (vm.Animal.Estado)
            {
                case EstadoAnimal.EnAdopcion:
                    ModelState.Remove("AnimalEnTransito.AnimalId");
                    ModelState.Remove("AnimalComunitario.AnimalId");
                    ModelState.Remove("AnimalComunitario.LugarHabitual");
                    break;

                case EstadoAnimal.EnTransito:
                    ModelState.Remove("AnimalEnAdopcion.AnimalId");
                    ModelState.Remove("AnimalComunitario.AnimalId");
                    ModelState.Remove("AnimalComunitario.LugarHabitual");
                    break;

                case EstadoAnimal.Comunitario:
                    ModelState.Remove("AnimalEnAdopcion.AnimalId");
                    ModelState.Remove("AnimalEnTransito.AnimalId");
                    break;
            }

            // ✅ Validación para evitar FK en adopción (si familia es obligatoria)
            if (vm.Animal.Estado == EstadoAnimal.EnAdopcion)
            {
                if (vm.AnimalEnAdopcion.FamiliaId == null || vm.AnimalEnAdopcion.FamiliaId == 0)
                    ModelState.AddModelError("AnimalEnAdopcion.FamiliaId", "Seleccione una familia válida.");
            }

            // ✅ Validación para comunitario (si lo querés obligatorio)
            if (vm.Animal.Estado == EstadoAnimal.Comunitario)
            {
                if (string.IsNullOrWhiteSpace(vm.AnimalComunitario.LugarHabitual))
                    ModelState.AddModelError("AnimalComunitario.LugarHabitual", "Ingrese el lugar habitual.");
            }

            if (!ModelState.IsValid)
                return View(vm);

            // 1) Guardar ANIMAL base
            _context.Animales.Add(vm.Animal);
            await _context.SaveChangesAsync(); // ya tenemos vm.Animal.Id

            // 2) Guardar SUBTIPO según estado
            if (vm.Animal.Estado == EstadoAnimal.EnTransito)
            {
                vm.AnimalEnTransito.AnimalId = vm.Animal.Id;

                // defaults por si viene vacío
                if (vm.AnimalEnTransito.FechaIngreso == default)
                    vm.AnimalEnTransito.FechaIngreso = DateTime.Today;

                _context.AnimalesEnTransito.Add(vm.AnimalEnTransito);
            }
            else if (vm.Animal.Estado == EstadoAnimal.EnAdopcion)
            {
                vm.AnimalEnAdopcion.AnimalId = vm.Animal.Id;

                if (vm.AnimalEnAdopcion.FechaAdopcion == default)
                    vm.AnimalEnAdopcion.FechaAdopcion = DateTime.Today;

                // ✅ Si llega 0, no guardes (pero esto debería estar cubierto por ModelState)
                if (vm.AnimalEnAdopcion.FamiliaId == 0)
                {
                    ModelState.AddModelError("AnimalEnAdopcion.FamiliaId", "Seleccione una familia válida.");
                    return View(vm);
                }

                _context.AnimalesEnAdopcion.Add(vm.AnimalEnAdopcion);
            }

            else if (vm.Animal.Estado == EstadoAnimal.Comunitario)
            {
                vm.AnimalComunitario.AnimalId = vm.Animal.Id;

                _context.AnimalesComunitarios.Add(vm.AnimalComunitario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // UPDATE (GET)
        // ==========================
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _context.Animales.FindAsync(id);
            if (animal == null) return NotFound();

            return View(animal);
        }

        // ==========================
        // UPDATE (POST)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,Nombre,Tipo,Raza,Edad,Foto,Estado")] Animal animal)
        {
            if (id != animal.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(animal);

            try
            {
                _context.Update(animal);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(animal.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // DELETE (GET)
        // ==========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var animal = await _context.Animales.FirstOrDefaultAsync(a => a.Id == id);
            if (animal == null) return NotFound();

            return View(animal);
        }

        // ==========================
        // DELETE (POST)
        // ==========================
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
            => _context.Animales.Any(e => e.Id == id);
    }
}