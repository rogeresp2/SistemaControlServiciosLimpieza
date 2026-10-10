using SistemaControlServiciosLimpieza.Api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{

    [ApiController]
    [Route("api/servicios")]
    public class ServiciosController : ControllerBase
    {
        private static readonly List<Servicio> _servicios = new List<Servicio>
        {
            new Servicio { Id = 1, Name = "Limpieza residencial", ServiceDate = new DateTime(2026, 10, 10), Cost = 2500.00m, Status = "Pendiente", ClienteId = 1 },
            new Servicio { Id = 2, Name = "Limpieza de oficina", ServiceDate = new DateTime(2026, 10, 12), Cost = 4500.00m, Status = "En proceso", ClienteId = 2 },
            new Servicio { Id = 3, Name = "Limpieza profunda", ServiceDate = new DateTime(2026, 10, 15), Cost = 3500.00m, Status = "Completado", ClienteId = 1 }
        };

        [HttpGet] // GET: api/servicios
        public ActionResult<IEnumerable<Servicio>> GetAll()
        {
            // Retornamos 200 OK con la lista completa.
            return Ok(_servicios);
        }

        [HttpGet("{id}")] // GET: api/servicios/5
        public ActionResult<Servicio> GetById(int id)
        {
            var servicio = _servicios.FirstOrDefault(s => s.Id == id);

            if (servicio == null)
            {
                // Retornar 404 si no se encontró
                return NotFound();
            }

            return Ok(servicio);
        }

        [HttpPost] // POST: api/servicios
        public ActionResult<Servicio> Create(Servicio servicio)
        {
            // Validación manual adicional: nombre no vacío.
            if (string.IsNullOrWhiteSpace(servicio.Name))
            {
                return BadRequest("Name of service is required.");
            }

            int newId = _servicios.Any() ? _servicios.Max(s => s.Id) + 1 : 1;
            servicio.Id = newId;

            _servicios.Add(servicio);

            // Devolver respuesta 201 Created con el recurso creado
            return CreatedAtAction(
                nameof(GetById),
                new { id = servicio.Id },
                servicio
            );
        }

        [HttpPut("{id}")] // PUT: api/servicios/5
        public IActionResult Update(int id, Servicio servicio)
        {
            var existing = _servicios.FirstOrDefault(s => s.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            // Actualizar propiedades (excepto el Id)
            existing.Name = servicio.Name;
            existing.ServiceDate = servicio.ServiceDate;
            existing.Cost = servicio.Cost;
            existing.Status = servicio.Status;
            existing.ClienteId = servicio.ClienteId;

            // Retornar 204 NoContent indicando que se realizó la operación sin devolver cuerpo.
            return NoContent();
        }

        [HttpDelete("{id}")] // DELETE: api/servicios/5
        public IActionResult Delete(int id)
        {
            var existing = _servicios.FirstOrDefault(s => s.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            _servicios.Remove(existing);

            // Retornamos 204 NoContent para indicar que se eliminó correctamente.
            return NoContent();
        }
    }
}
