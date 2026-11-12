using System.ComponentModel.DataAnnotations;

namespace SistemaControlServiciosLimpieza.Api.Models.Entities
{
    public class Servicio
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public decimal Cost { get; set; }

        [StringLength(30)]
        public string Status { get; set; } = string.Empty;
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}
