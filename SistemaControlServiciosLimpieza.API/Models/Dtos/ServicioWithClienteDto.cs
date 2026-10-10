namespace SistemaControlServiciosLimpieza.Api.Models.Dtos
{
    // DTO de salida para Servicio con información de Cliente
    public class ServicioWithClienteDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public string ClienteName { get; set; } = string.Empty;
    }
}