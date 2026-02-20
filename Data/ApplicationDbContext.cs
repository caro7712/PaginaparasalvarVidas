using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Models;
using PaginaparaSalvarVidas.Models.Procedimiento_almacenado;

namespace PaginaparaSalvarVidas.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animales { get; set; }
        public DbSet<AnimalComunitario> AnimalesComunitarios { get; set; }
        public DbSet<AnimalEnAdopcion> AnimalesEnAdopcion { get; set; }
        public DbSet<AnimalEnTransito> AnimalesEnTransito { get; set; }
        public DbSet<AnimalPerdidoEncontrado> AnimalesPerdidosEncontrados { get; set; }
        public DbSet<Familia> Familias { get; set; }
        public DbSet<FiltrarAnimal> FiltrarAnimales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FiltrarAnimal>().HasNoKey();
        }
    }

}