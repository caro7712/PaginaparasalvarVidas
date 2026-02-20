using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaginaparaSalvarVidas.Models;

namespace PaginaparaSalvarVidas.Data
{
    public static class Semilla
    {
        public static async Task Inicializar(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // ← NUEVO: obtener UserManager y RoleManager del DI
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // -----------------------------
            // ROL ADMIN
            // -----------------------------
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("Usuario"))
                await roleManager.CreateAsync(new IdentityRole("Usuario"));

            // -----------------------------
            // USUARIO ADMIN
            // -----------------------------
            const string adminEmail = "admin@salvar.com";
            const string adminPassword = "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true // ← evita que pida confirmar email para poder loguearse
                };

                var resultado = await userManager.CreateAsync(admin, adminPassword);

                if (resultado.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }

            // -----------------------------
            // DATOS DE NEGOCIO
            // -----------------------------
            if (context.Familias.Any())
                return;

            var familia = new Familia
            {
                Nombre = "Familia González",
                Direccion = "Av. Siempre Viva 742",
                Telefono = "1122334455"
            };
            context.Familias.Add(familia);
            await context.SaveChangesAsync();

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

            context.AnimalesEnAdopcion.Add(new AnimalEnAdopcion
            {
                AnimalId = animal.Id,
                FamiliaId = familia.Id,
                FechaAdopcion = DateTime.Now
            });

            context.AnimalesEnTransito.Add(new AnimalEnTransito
            {
                AnimalId = animal.Id,
                FamiliaId = familia.Id,
                FechaIngreso = DateTime.Now,
                FechaSalida = null
            });

            context.AnimalesPerdidosEncontrados.Add(new AnimalPerdidoEncontrado
            {
                AnimalId = animal.Id,
                Descripcion = "Perro perdido en parque central",
                Direccion = "Parque Central",
                TelefonoContacto = "11999888777",
                Fecha = DateTime.Now
            });

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