using PaginaparaSalvarVidas.Models;
using System.ComponentModel.DataAnnotations;

namespace PaginaparaSalvarVidas.Models
{
    public class AnimalComunitario
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
        // DATOS ESPECÍFICOS
        // ==========================
        [Required(ErrorMessage = "Ingrese el lugar habitual")]
        [StringLength(150)]
        public string LugarHabitual { get; set; } = string.Empty;

        [Display(Name = "Apto para adopción")]
        public bool AptoParaAdopcion { get; set; }
    }
}