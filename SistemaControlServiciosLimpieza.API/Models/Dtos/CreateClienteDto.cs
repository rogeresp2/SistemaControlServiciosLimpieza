namespace SistemaControlServiciosLimpieza.Api.Models.Dtos
{
    public class CreateClienteDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}