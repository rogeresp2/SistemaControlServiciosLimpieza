namespace SistemaControlServiciosLimpieza.Api.Models.Dtos
{
    public class ServicioDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ClienteId { get; set; }
    }
}