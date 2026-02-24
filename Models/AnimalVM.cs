using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaginaparaSalvarVidas.Models
{

    public class AnimalVM
    {
        // ===== ENTIDAD BASE =====
        [ValidateNever] // ⭐⭐⭐ CLAVE
        public Animal Animal { get; set; } = new();

        // ===== SUBTIPOS =====
        [ValidateNever]
        public AnimalEnAdopcion AnimalEnAdopcion { get; set; } = new();

        [ValidateNever]
        public AnimalEnTransito AnimalEnTransito { get; set; } = new();

        [ValidateNever]
        public AnimalComunitario AnimalComunitario { get; set; } = new();

        // ===== LISTAS =====
        public List<SelectListItem> Familias { get; set; } = new();
    }
}