using PaginaparaSalvarVidas.Models;
using System.ComponentModel.DataAnnotations;

namespace PaginaparaSalvarVidas.Models
{
    public class AnimalEnAdopcion
    {
        // ==========================
        // CLAVE PRIMARIA
        // ==========================
        [Key]
        public int Id { get; set; }

        // ==========================
        // RELACIÓN 1 a 1 - ANIMAL
        // ==========================
        [Required]
        public int AnimalId { get; set; }

        public Animal Animal { get; set; } = null!;

        // ==========================
        // RELACIÓN - FAMILIA
        // ==========================
        [Required]
        public int FamiliaId { get; set; }

        public Familia Familia { get; set; } = null!;

        // ==========================
        // DATOS ESPECÍFICOS
        // ==========================
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de adopción")]
        public DateTime FechaAdopcion { get; set; }
    }
}