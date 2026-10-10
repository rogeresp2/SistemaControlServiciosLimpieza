namespace SistemaControlServiciosLimpieza.Api.Models.Dtos
{
    public class CreateEmpleadoDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
    }
}