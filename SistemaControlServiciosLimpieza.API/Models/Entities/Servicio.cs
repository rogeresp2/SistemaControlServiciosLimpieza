namespace SistemaControlServiciosLimpieza.Api.Models.Entities
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ClienteId { get; set; }
    }
}
