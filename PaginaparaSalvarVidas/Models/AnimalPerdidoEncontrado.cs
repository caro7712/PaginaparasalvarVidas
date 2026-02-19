using PaginaparaSalvarVidas.Models;
using System.ComponentModel.DataAnnotations;

namespace PaginaparaSalvarVidas.Models
{
    public class AnimalPerdidoEncontrado
    {
        // ==========================
        // CLAVE PRIMARIA
        // ==========================
        [Key]
        public int Id { get; set; }

        // ==========================
        // RELACIÓN OPCIONAL - ANIMAL
        // ==========================
        public int? AnimalId { get; set; }
        public Animal? Animal { get; set; }

        // ==========================
        // DATOS DEL REPORTE
        // ==========================
        [Required(ErrorMessage = "Ingrese una descripción")]
        [StringLength(300)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la dirección")]
        [StringLength(150)]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese un teléfono de contacto")]
        [Phone]
        public string TelefonoContacto { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
    }
}
