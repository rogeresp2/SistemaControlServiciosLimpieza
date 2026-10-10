namespace SistemaControlServiciosLimpieza.Api.Models.Dtos
{
    // DTO de salida para Asignacion con información de Servicio y Empleado
    public class AsignacionWithDetailsDto
    {
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public string ServicioName { get; set; } = string.Empty;
        public int EmpleadoId { get; set; }
        public string EmpleadoName { get; set; } = string.Empty;
        public DateTime AssignmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}