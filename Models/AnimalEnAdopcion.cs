using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        // RELACIÓN - ANIMAL
        // ==========================
        [Required]
        public int AnimalId { get; set; }

        public Animal? Animal { get; set; }

        // ==========================
        // RELACIÓN - FAMILIA
        // ==========================
        [Required]
        public int FamiliaId { get; set; }

        [ValidateNever]
        public Familia? Familia { get; set; }

        // ==========================
        // DATOS ESPECÍFICOS
        // ==========================
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de adopción")]
        public DateTime FechaAdopcion { get; set; }
    }
}