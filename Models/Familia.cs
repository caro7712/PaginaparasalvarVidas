using System.ComponentModel.DataAnnotations;

namespace PaginaparaSalvarVidas.Models
{
    public class Familia
    {
        // ==========================
        // CLAVE PRIMARIA
        // ==========================
        [Key]
        public int Id { get; set; }

        // ==========================
        // DATOS GENERALES
        // ==========================
        [Required(ErrorMessage = "Ingrese el nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la dirección")]
        [StringLength(150)]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese el teléfono")]
        [Phone]
        public string Telefono { get; set; } = string.Empty;

        // ==========================
        // RELACIONES
        // ==========================
        public ICollection<AnimalEnTransito> AnimalesEnTransito { get; set; } = new List<AnimalEnTransito>();

        public ICollection<AnimalEnAdopcion> AnimalesAdoptados { get; set; } = new List<AnimalEnAdopcion>();
    }
}