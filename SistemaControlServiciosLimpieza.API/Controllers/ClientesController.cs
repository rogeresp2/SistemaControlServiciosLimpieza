using SistemaControlServiciosLimpieza.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{

    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private static readonly List<Cliente> _clientes = new List<Cliente>
        {
            new Cliente { Id = 1, Name = "Juan Pérez", Phone = "809-555-1234", Email = "juan@example.com", IsActive = true },
            new Cliente { Id = 2, Name = "María Rodríguez", Phone = "829-555-5678", Email = "maria@example.com", IsActive = true },
            new Cliente { Id = 3, Name = "Carlos Martínez", Phone = "849-555-9012", Email = "carlos@example.com", IsActive = true }
        };

        [HttpGet] // GET: api/clientes
        public ActionResult<IEnumerable<Cliente>> GetAll()
        {
            // Retornamos 200 OK con la lista completa.
            return Ok(_clientes);
        }

        [HttpGet("{id}")] // GET: api/clientes/5
        public ActionResult<Cliente> GetById(int id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                // Retornar 404 si no se encontró
                return NotFound();
            }

            return Ok(cliente);
        }

        [HttpPost] // POST: api/clientes
        public ActionResult<Cliente> Create(Cliente cliente)
        {
            // Validación manual adicional: nombre no vacío (alternativa a [Required]).
            if (string.IsNullOrWhiteSpace(cliente.Name))
            {
                return BadRequest("Name of client is required.");
            }

            int newId = _clientes.Any() ? _clientes.Max(c => c.Id) + 1 : 1;
            cliente.Id = newId;

            if (cliente.IsActive == false)
            {
                // Por lógica de negocio, podríamos decidir que todo nuevo cliente inicia activo.
                cliente.IsActive = true;
            }

            _clientes.Add(cliente);

            // Devolver respuesta 201 Created con el recurso creado
            return CreatedAtAction(
                nameof(GetById),              // Nombre de la acción para generar el link de detalle
                new { id = cliente.Id },      // Valores de ruta (el id del nuevo recurso)
                cliente                       // El objeto creado (en el cuerpo de la respuesta)
            );
        }

        [HttpPut("{id}")] // PUT: api/clientes/5
        public IActionResult Update(int id, Cliente cliente)
        {
            var existing = _clientes.FirstOrDefault(c => c.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            // Opcional: validar que cliente.Id == id si quisiéramos forzar consistencia.
            // Actualizar propiedades (excepto el Id)
            existing.Name = cliente.Name;
            existing.Phone = cliente.Phone;
            existing.Email = cliente.Email;
            existing.IsActive = cliente.IsActive;

            // Retornar 204 NoContent indicando que se realizó la operación sin devolver cuerpo.
            return NoContent();
        }

        [HttpDelete("{id}")] // DELETE: api/clientes/5
        public IActionResult Delete(int id)
        {
            var existing = _clientes.FirstOrDefault(c => c.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            _clientes.Remove(existing);

            // Retornamos 204 NoContent para indicar que se eliminó correctamente (sin contenido).
            return NoContent();
        }
    }
}