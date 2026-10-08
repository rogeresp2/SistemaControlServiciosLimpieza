using SistemaControlServiciosLimpieza.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{

    [ApiController]
    [Route("api/empleados")]
    public class EmpleadosController : ControllerBase
    {
        private static readonly List<Empleado> _empleados = new List<Empleado>
        {
            new Empleado { Id = 1, Name = "Carlos Rodríguez", Phone = "809-555-1234", Email = "carlos@example.com", Position = "Supervisor", IsActive = true },
            new Empleado { Id = 2, Name = "Ana Martínez", Phone = "829-555-5678", Email = "ana@example.com", Position = "Limpieza", IsActive = true },
            new Empleado { Id = 3, Name = "Luis García", Phone = "849-555-9012", Email = "luis@example.com", Position = "Coordinador", IsActive = true }
        };

        [HttpGet] // GET: api/empleados
        public ActionResult<IEnumerable<Empleado>> GetAll()
        {
            // Retornamos 200 OK con la lista completa.
            return Ok(_empleados);
        }

        [HttpGet("{id}")] // GET: api/empleados/5
        public ActionResult<Empleado> GetById(int id)
        {
            var empleado = _empleados.FirstOrDefault(e => e.Id == id);

            if (empleado == null)
            {
                // Retornar 404 si no se encontró
                return NotFound();
            }

            return Ok(empleado);
        }

        [HttpPost] // POST: api/empleados
        public ActionResult<Empleado> Create(Empleado empleado)
        {
            // Validación manual adicional: nombre no vacío (alternativa a [Required]).
            if (string.IsNullOrWhiteSpace(empleado.Name))
            {
                return BadRequest("Name of employee is required.");
            }

            int newId = _empleados.Any() ? _empleados.Max(e => e.Id) + 1 : 1;
            empleado.Id = newId;

            if (empleado.IsActive == false)
            {
                // Por lógica de negocio, podríamos decidir que todo nuevo empleado inicia activo.
                empleado.IsActive = true;
            }

            _empleados.Add(empleado);

            // Devolver respuesta 201 Created con el recurso creado
            return CreatedAtAction(
                nameof(GetById),
                new { id = empleado.Id },
                empleado
            );
        }

        [HttpPut("{id}")] // PUT: api/empleados/5
        public IActionResult Update(int id, Empleado empleado)
        {
            var existing = _empleados.FirstOrDefault(e => e.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            // Opcional: validar que empleado.Id == id si quisiéramos forzar consistencia.
            // Actualizar propiedades (excepto el Id)
            existing.Name = empleado.Name;
            existing.Phone = empleado.Phone;
            existing.Email = empleado.Email;
            existing.Position = empleado.Position;
            existing.IsActive = empleado.IsActive;

            // Retornar 204 NoContent indicando que se realizó la operación sin devolver cuerpo.
            return NoContent();
        }

        [HttpDelete("{id}")] // DELETE: api/empleados/5
        public IActionResult Delete(int id)
        {
            var existing = _empleados.FirstOrDefault(e => e.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            _empleados.Remove(existing);

            // Retornamos 204 NoContent para indicar que se eliminó correctamente (sin contenido).
            return NoContent();
        }
    }
}