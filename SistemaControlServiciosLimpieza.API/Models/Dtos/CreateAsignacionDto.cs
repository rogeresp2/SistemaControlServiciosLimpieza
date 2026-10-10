namespace SistemaControlServiciosLimpieza.Api.Models.Dtos
{
    public class CreateAsignacionDto
    {
        public int ServicioId { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}