using SistemaControlServiciosLimpieza.Api.Data;
using SistemaControlServiciosLimpieza.Api.Models.Entities;
using SistemaControlServiciosLimpieza.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/empleados")]
    public class EmpleadosController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Contexto de BD para acceder a las tablas

        public EmpleadosController(ApplicationDbContext context)
        {
            _context = context;
            // Nota: No inicializamos lista estática, los datos están en la BD.
        }

        // GET: api/empleados
        [HttpGet]
        public ActionResult<List<EmpleadoDto>> GetAll()
        {
            // Recupera los empleados (entidades) de la base de datos
            var empleados = _context.Empleados.ToList();

            // Mapeo manual de entidades Empleado a DTOs EmpleadoDto
            var empleadoDtos = empleados.Select(e => new EmpleadoDto
            {
                Id = e.Id,
                Name = e.Name,
                Phone = e.Phone,
                Email = e.Email,
                Position = e.Position,
                IsActive = e.IsActive
            }).ToList();

            return Ok(empleadoDtos);
        }

        // GET: api/empleados/{id}
        [HttpGet("{id}")]
        public ActionResult<EmpleadoDto> GetById(int id)
        {
            var empleado = _context.Empleados.Find(id);

            if (empleado == null)
                return NotFound();

            // Mapeo de la entidad encontrada a DTO
            var empleadoDto = new EmpleadoDto
            {
                Id = empleado.Id,
                Name = empleado.Name,
                Phone = empleado.Phone,
                Email = empleado.Email,
                Position = empleado.Position,
                IsActive = empleado.IsActive
            };

            return Ok(empleadoDto);
        }

        // POST: api/empleados
        [HttpPost]
        public ActionResult<Empleado> Create([FromBody] CreateEmpleadoDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("El nombre del empleado es obligatorio.");

            var empleado = new Empleado
            {
                Name = request.Name,
                Phone = request.Phone,
                Email = request.Email,
                Position = request.Position,
                IsActive = true
            };

            _context.Empleados.Add(empleado);
            _context.SaveChanges();

            return Ok(new { empleado.Id });
        }

        // PUT: api/empleados/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] EmpleadoDto request)
        {
            if (id != request.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("El nombre del empleado no puede estar vacío.");

            var existingEmpleado = _context.Empleados.Find(id);

            if (existingEmpleado == null)
                return NotFound();

            existingEmpleado.Name = request.Name;
            existingEmpleado.Phone = request.Phone;
            existingEmpleado.Email = request.Email;
            existingEmpleado.Position = request.Position;
            existingEmpleado.IsActive = request.IsActive;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/empleados/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var empleado = _context.Empleados.Find(id);

            if (empleado == null)
                return NotFound();

            _context.Empleados.Remove(empleado);
            _context.SaveChanges();

            return NoContent();
        }
    }
}