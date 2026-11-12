using System.ComponentModel.DataAnnotations;

namespace SistemaControlServiciosLimpieza.Api.Models.Entities
{
    public class Asignacion
    {
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public Servicio? Servicio { get; set; }
        public int EmpleadoId { get; set; }
        public Empleado? Empleado { get; set; }
        public DateTime AssignmentDate { get; set; }

        [StringLength(30)]
        public string Status { get; set; } = string.Empty;
    }
}
