namespace SistemaControlServiciosLimpieza.Api.Models.Dtos
{
    public class AsignacionDto
    {
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}