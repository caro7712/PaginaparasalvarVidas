namespace PaginaparaSalvarVidas.Models.Procedimiento_almacenado
{
    public class FiltrarAnimal
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string? Raza { get; set; }
        public int Edad { get; set; }
        public string? Foto { get; set; }
        public EstadoAnimal Estado { get; set; }
    }
}