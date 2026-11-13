using SistemaControlServiciosLimpieza.Api.Data;
using SistemaControlServiciosLimpieza.Api.Models.Entities;
using SistemaControlServiciosLimpieza.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Contexto de BD para acceder a las tablas
        private readonly IMapper _mapper;
        public ClientesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            // Nota: No inicializamos lista estática, los datos están en la BD.
            _mapper = mapper;
        }


        // GET: api/clientes
        [HttpGet]
        public ActionResult<List<ClienteDto>> GetAll()
        {
            // Recupera los clientes (entidades) de la base de datos
            var clientes = _context.Clientes.ToList();

            // Mapeo manual de entidades Cliente a DTOs ClienteDto
            //var clienteDtos = clientes.Select(c => new ClienteDto
            //{
            //    Id = c.Id,
            //    Name = c.Name,
            //    Phone = c.Phone,
            //    Email = c.Email,
            //    IsActive = c.IsActive
            //}).ToList();

            var clienteDtos = _mapper.Map<List<ClienteDto>>(clientes);

            return Ok(clienteDtos);
        }

        // GET: api/clientes/{id}
        [HttpGet("{id}")]
        public ActionResult<ClienteDto> GetById(int id)
        {
            var cliente = _context.Clientes.Find(id);

            if (cliente == null)
                return NotFound();

            //// Mapeo de la entidad encontrada a DTO
            //var clienteDto = new ClienteDto
            //{
            //    Id = cliente.Id,
            //    Name = cliente.Name,
            //    Phone = cliente.Phone,
            //    Email = cliente.Email,
            //    IsActive = cliente.IsActive
            //};

            var clienteDto = _mapper.Map<ClienteDto>(cliente);

            return Ok(clienteDto);
        }


        // POST: api/clientes
        [HttpPost]
        public ActionResult<Cliente> Create([FromBody] CreateClienteDto request)
        {
            // Validación básica de negocio
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("El nombre del cliente es obligatorio.");
            }

            //var cliente = new Cliente
            //{
            //    Name = request.Name,
            //    Phone = request.Phone,
            //    Email = request.Email,  
            //    IsActive = true,
            //};

            var cliente = _mapper.Map<Cliente>(request);
            cliente.IsActive = true;

            // Preparar entidad Cliente para guardar (la BD asignará el Id automáticamente)

            _context.Clientes.Add(cliente);
            _context.SaveChanges(); // Ejecuta el INSERT en la BD

            // cliente.Id ahora tiene el valor generado en la base de datos (Identity)

            // Devolver respuesta 201 Created con el recurso creado
            return Ok(
                new { cliente.Id }      // Cuerpo de la respuesta: cliente creado
            );
        }

        // PUT: api/clientes/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ClienteDto request)
        {
            if (id != request.Id)
            {
                // El Id de la URL no coincide con el Id del cuerpo
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("El nombre del cliente no puede estar vacío.");
            }

            // Verificar existencia del cliente a actualizar
            var existingCliente = _context.Clientes.Find(id);

            if (existingCliente == null)
            {
                return NotFound();
            }

            //// Actualizar campos del cliente existente
            //existingCliente.Name = request.Name;
            //existingCliente.Phone = request.Phone;
            //existingCliente.Email = request.Email;
            //existingCliente.IsActive = request.IsActive;

            _mapper.Map(request, existingCliente);

            _context.SaveChanges(); // Aplicar cambios en la base de datos (UPDATE)

            return NoContent(); // 204: actualización exitosa sin contenido adicional
        }

        // DELETE: api/clientes/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);

            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            _context.SaveChanges(); // Ejecuta el DELETE en la BD

            return NoContent(); // 204: se eliminó correctamente
        }
    }
}
