using SistemaControlServiciosLimpieza.Api.Data;
using SistemaControlServiciosLimpieza.Api.Models.Entities;
using SistemaControlServiciosLimpieza.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/asignaciones")]
    public class AsignacionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Contexto de BD para acceder a las tablas

        public AsignacionesController(ApplicationDbContext context)
        {
            _context = context;
            // Nota: No inicializamos lista estática, los datos están en la BD.
        }

        // GET: api/asignaciones
        [HttpGet]
        public ActionResult<List<AsignacionDto>> GetAll()
        {
            // Recupera las asignaciones (entidades) de la base de datos
            var asignaciones = _context.Asignaciones.ToList();

            // Mapeo manual de entidades Asignacion a DTOs AsignacionDto
            var asignacionDtos = asignaciones.Select(a => new AsignacionDto
            {
                Id = a.Id,
                ServicioId = a.ServicioId,
                EmpleadoId = a.EmpleadoId,
                AssignmentDate = a.AssignmentDate,
                Status = a.Status
            }).ToList();

            return Ok(asignacionDtos);
        }

        // GET: api/asignaciones/{id}
        [HttpGet("{id}")]
        public ActionResult<AsignacionDto> GetById(int id)
        {
            var asignacion = _context.Asignaciones.Find(id);

            if (asignacion == null)
                return NotFound();

            // Mapeo de la entidad encontrada a DTO
            var asignacionDto = new AsignacionDto
            {
                Id = asignacion.Id,
                ServicioId = asignacion.ServicioId,
                EmpleadoId = asignacion.EmpleadoId,
                AssignmentDate = asignacion.AssignmentDate,
                Status = asignacion.Status
            };

            return Ok(asignacionDto);
        }

        // GET /api/asignaciones/con-detalles
        [HttpGet("con-detalles")]
        public ActionResult<List<AsignacionWithDetailsDto>> GetAsignacionesWithDetails()
        {
            var asignaciones = _context.Asignaciones
                .Include(a => a.Servicio)
                .Include(a => a.Empleado)
                .ToList();

            // Mapeo manual de entidades Asignacion a DTOs AsignacionWithDetailsDto
            var asignacionDTOs = asignaciones.Select(a => new AsignacionWithDetailsDto
            {
                Id = a.Id,
                ServicioId = a.ServicioId,
                ServicioName = a.Servicio != null ? a.Servicio.Name : "",
                EmpleadoId = a.EmpleadoId,
                EmpleadoName = a.Empleado != null ? a.Empleado.Name : "",
                AssignmentDate = a.AssignmentDate,
                Status = a.Status
            }).ToList();

            return Ok(asignacionDTOs);
        }

        // POST: api/asignaciones
        [HttpPost]
        public ActionResult<Asignacion> Create([FromBody] CreateAsignacionDto request)
        {
            if (!_context.Servicios.Any(s => s.Id == request.ServicioId))
                return BadRequest("El servicio indicado no existe.");

            if (!_context.Empleados.Any(e => e.Id == request.EmpleadoId))
                return BadRequest("El empleado indicado no existe.");

            var asignacion = new Asignacion
            {
                ServicioId = request.ServicioId,
                EmpleadoId = request.EmpleadoId,
                AssignmentDate = request.AssignmentDate,
                Status = request.Status
            };

            _context.Asignaciones.Add(asignacion);
            _context.SaveChanges();

            return Ok(new { asignacion.Id });
        }

        // PUT: api/asignaciones/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AsignacionDto request)
        {
            if (id != request.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");

            if (!_context.Servicios.Any(s => s.Id == request.ServicioId))
                return BadRequest("El servicio indicado no existe.");

            if (!_context.Empleados.Any(e => e.Id == request.EmpleadoId))
                return BadRequest("El empleado indicado no existe.");

            var existingAsignacion = _context.Asignaciones.Find(id);

            if (existingAsignacion == null)
                return NotFound();

            existingAsignacion.ServicioId = request.ServicioId;
            existingAsignacion.EmpleadoId = request.EmpleadoId;
            existingAsignacion.AssignmentDate = request.AssignmentDate;
            existingAsignacion.Status = request.Status;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/asignaciones/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var asignacion = _context.Asignaciones.Find(id);

            if (asignacion == null)
                return NotFound();

            _context.Asignaciones.Remove(asignacion);
            _context.SaveChanges();

            return NoContent();
        }
    }
}