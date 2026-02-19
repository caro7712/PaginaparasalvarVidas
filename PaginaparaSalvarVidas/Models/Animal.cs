using System.ComponentModel.DataAnnotations;

namespace PaginaparaSalvarVidas.Models
{
    public class Animal
    {
        // ==========================
        // CLAVE PRIMARIA
        // ==========================
        [Key]
        public int Id { get; set; }

        // ==========================
        // DATOS GENERALES
        // ==========================
        [Required(ErrorMessage = "Ingrese el nombre del animal")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el tipo de animal")]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Raza { get; set; }

        [Range(0, 30, ErrorMessage = "La edad debe estar entre 0 y 30 años")]
        public int Edad { get; set; }

        [Display(Name = "Foto")]
        public string? Foto { get; set; }

        // ==========================
        // ESTADO
        // ==========================
        [Required]
        public EstadoAnimal Estado { get; set; }

        // ==========================
        // RELACIONES (1 a 1)
        // ==========================
        public AnimalEnTransito? AnimalEnTransito { get; set; }
        public AnimalEnAdopcion? AnimalEnAdopcion { get; set; }
        public AnimalComunitario? AnimalComunitario { get; set; }
    }
}


