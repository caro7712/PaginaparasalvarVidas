using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaginaparaSalvarVidas.Models
{
    public class AnimalEnAdopcionVM
    {
        public AnimalEnAdopcion AnimalEnAdopcion { get; set; } = new();

        public List<SelectListItem> Animales { get; set; } = new();
        public List<SelectListItem> Familias { get; set; } = new();
    }
}