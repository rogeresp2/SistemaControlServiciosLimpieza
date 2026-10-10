using SistemaControlServiciosLimpieza.Api.Data;
using SistemaControlServiciosLimpieza.Api.Models.Entities;
using SistemaControlServiciosLimpieza.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/servicios")]
    public class ServiciosController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Contexto de BD para acceder a las tablas

        public ServiciosController(ApplicationDbContext context)
        {
            _context = context;
            // Nota: No inicializamos lista estática, los datos están en la BD.
        }

        // GET: api/servicios
        [HttpGet]
        public ActionResult<List<ServicioDto>> GetAll()
        {
            // Recupera los servicios (entidades) de la base de datos
            var servicios = _context.Servicios.ToList();

            // Mapeo manual de entidades Servicio a DTOs ServicioDto
            var servicioDtos = servicios.Select(s => new ServicioDto
            {
                Id = s.Id,
                Name = s.Name,
                ServiceDate = s.ServiceDate,
                Cost = s.Cost,
                Status = s.Status,
                ClienteId = s.ClienteId
            }).ToList();

            return Ok(servicioDtos);
        }

        // GET: api/servicios/{id}
        [HttpGet("{id}")]
        public ActionResult<ServicioDto> GetById(int id)
        {
            var servicio = _context.Servicios.Find(id);

            if (servicio == null)
                return NotFound();

            // Mapeo de la entidad encontrada a DTO
            var servicioDto = new ServicioDto
            {
                Id = servicio.Id,
                Name = servicio.Name,
                ServiceDate = servicio.ServiceDate,
                Cost = servicio.Cost,
                Status = servicio.Status,
                ClienteId = servicio.ClienteId
            };

            return Ok(servicioDto);
        }

        // GET /api/servicios/con-cliente
        [HttpGet("con-cliente")]
        public ActionResult<List<ServicioWithClienteDto>> GetServiciosWithCliente()
        {
            var servicios = _context.Servicios
                .Include(s => s.Cliente)
                .ToList();

            // Mapeo manual de entidades Servicio a DTOs ServicioWithClienteDto
            var servicioDTOs = servicios.Select(s => new ServicioWithClienteDto
            {
                Id = s.Id,
                Name = s.Name,
                ServiceDate = s.ServiceDate,
                Cost = s.Cost,
                Status = s.Status,
                ClienteId = s.ClienteId,
                ClienteName = s.Cliente != null ? s.Cliente.Name : ""
            }).ToList();

            return Ok(servicioDTOs);
        }

        // POST: api/servicios
        [HttpPost]
        public ActionResult<Servicio> Create([FromBody] CreateServicioDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("El nombre del servicio es obligatorio.");

            if (!_context.Clientes.Any(c => c.Id == request.ClienteId))
                return BadRequest("El cliente indicado no existe.");

            var servicio = new Servicio
            {
                Name = request.Name,
                ServiceDate = request.ServiceDate,
                Cost = request.Cost,
                Status = request.Status,
                ClienteId = request.ClienteId
            };

            _context.Servicios.Add(servicio);
            _context.SaveChanges();

            return Ok(new { servicio.Id });
        }

        // PUT: api/servicios/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ServicioDto request)
        {
            if (id != request.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("El nombre del servicio no puede estar vacío.");

            if (!_context.Clientes.Any(c => c.Id == request.ClienteId))
                return BadRequest("El cliente indicado no existe.");

            var existingServicio = _context.Servicios.Find(id);

            if (existingServicio == null)
                return NotFound();

            existingServicio.Name = request.Name;
            existingServicio.ServiceDate = request.ServiceDate;
            existingServicio.Cost = request.Cost;
            existingServicio.Status = request.Status;
            existingServicio.ClienteId = request.ClienteId;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/servicios/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var servicio = _context.Servicios.Find(id);

            if (servicio == null)
                return NotFound();

            _context.Servicios.Remove(servicio);
            _context.SaveChanges();

            return NoContent();
        }
    }
}