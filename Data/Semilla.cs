using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Models;

namespace PaginaparaSalvarVidas.Data
{
    public static class Semilla
    {
        public static async Task Inicializar(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Si ya hay datos, no vuelve a sembrar
            if (context.Familias.Any())
                return;

            // -----------------------------
            // FAMILIA
            // -----------------------------
            var familia = new Familia
            {
                Nombre = "Familia González",
                Direccion = "Av. Siempre Viva 742",
                Telefono = "1122334455"
            };

            context.Familias.Add(familia);
            await context.SaveChangesAsync();

            // -----------------------------
            // ANIMAL
            // -----------------------------
            var animal = new Animal
            {
                Nombre = "Luna",
                Tipo = "Perro",
                Raza = "Labrador",
                Edad = 3,
                Foto = "luna.jpg",
                Estado = EstadoAnimal.EnTransito
            };

            context.Animales.Add(animal);
            await context.SaveChangesAsync();

            // -----------------------------
            // ANIMAL EN ADOPCIÓN
            // -----------------------------
            context.AnimalesEnAdopcion.Add(new AnimalEnAdopcion
            {
                AnimalId = animal.Id,
                FamiliaId = familia.Id,
                FechaAdopcion = DateTime.Now
            });

            // -----------------------------
            // ANIMAL EN TRÁNSITO
            // -----------------------------
            context.AnimalesEnTransito.Add(new AnimalEnTransito
            {
                AnimalId = animal.Id,
                FamiliaId = familia.Id,
                FechaIngreso = DateTime.Now,
                FechaSalida = null
            });

            // -----------------------------
            // ANIMAL PERDIDO / ENCONTRADO
            // -----------------------------
            context.AnimalesPerdidosEncontrados.Add(new AnimalPerdidoEncontrado
            {
                AnimalId = animal.Id,
                Descripcion = "Perro perdido en parque central",
                Direccion = "Parque Central",
                TelefonoContacto = "11999888777",
                Fecha = DateTime.Now
            });

            // -----------------------------
            // ANIMAL COMUNITARIO
            // -----------------------------
            context.AnimalesComunitarios.Add(new AnimalComunitario
            {
                AnimalId = animal.Id,
                LugarHabitual = "Plaza San Martín",
                AptoParaAdopcion = true
            });

            await context.SaveChangesAsync();
        }
    }
}
