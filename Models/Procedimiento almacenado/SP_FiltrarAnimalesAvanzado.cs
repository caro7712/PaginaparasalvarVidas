using Microsoft.EntityFrameworkCore;
using PaginaparaSalvarVidas.Data;
using PaginaparaSalvarVidas.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaginaparaSalvarVidas.Models.Procedimiento_almacenado
{
    public class SP_FiltrarAnimalesAvanzado
    {
        private readonly ApplicationDbContext _context;

        public SP_FiltrarAnimalesAvanzado(ApplicationDbContext context)
        {
            _context = context;
        }

        // Simula el procedimiento almacenado
        public async Task<List<FiltrarAnimal>> Ejecutar(
            EstadoAnimal? estado = null,
            string? tipo = null,
            int? edadMin = null,
            int? edadMax = null)
        {
            var query = _context.Animales.AsQueryable();

            if (estado.HasValue)
                query = query.Where(a => a.Estado == estado.Value);

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(a => a.Tipo.Contains(tipo));

            if (edadMin.HasValue)
                query = query.Where(a => a.Edad >= edadMin.Value);

            if (edadMax.HasValue)
                query = query.Where(a => a.Edad <= edadMax.Value);

            // Proyectamos al modelo de salida
            return await query
                .Select(a => new FiltrarAnimal
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Tipo = a.Tipo,
                    Raza = a.Raza,
                    Edad = a.Edad,
                    Foto = a.Foto,
                    Estado = a.Estado
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}