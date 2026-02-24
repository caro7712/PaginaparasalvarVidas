using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace PaginaparaSalvarVidas.Models
{
    public class AnimalEnTransito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AnimalId { get; set; }

        public Animal? Animal { get; set; }

        // ✅ AHORA sí puede ser null
        public int? FamiliaId { get; set; }

        [ValidateNever]
        public Familia? Familia { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaIngreso { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaSalida { get; set; }
    }
}