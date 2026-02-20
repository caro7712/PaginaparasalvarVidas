namespace PaginaparaSalvarVidas.Models
{
    public class AnimalPerdidoEncontradoVM
    {
        public int Id { get; set; }

        public string? NombreAnimal { get; set; }

        public string? Estado { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public string TelefonoContacto { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

    }
}
