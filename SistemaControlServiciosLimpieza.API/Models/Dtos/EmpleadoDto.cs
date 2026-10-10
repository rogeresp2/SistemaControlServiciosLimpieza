namespace SistemaControlServiciosLimpieza.Api.Models.Dtos
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
        public bool IsActive { get; set; } = true;
    }
}