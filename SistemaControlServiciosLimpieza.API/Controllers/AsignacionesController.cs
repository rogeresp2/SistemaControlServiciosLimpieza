using SistemaControlServiciosLimpieza.Api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{

    [ApiController]
    [Route("api/asignaciones")]
    public class AsignacionesController : ControllerBase
    {
        private static readonly List<Asignacion> _asignaciones = new List<Asignacion>
        {
            new Asignacion { Id = 1, ServicioId = 1, EmpleadoId = 2, AssignmentDate = new DateTime(2026, 10, 9), Status = "Pendiente" },
            new Asignacion { Id = 2, ServicioId = 2, EmpleadoId = 1, AssignmentDate = new DateTime(2026, 10, 9), Status = "En proceso" },
            new Asignacion { Id = 3, ServicioId = 3, EmpleadoId = 3, AssignmentDate = new DateTime(2026, 10, 9), Status = "Completada" }
        };

        [HttpGet] // GET: api/asignaciones
        public ActionResult<IEnumerable<Asignacion>> GetAll()
        {
            // Retornamos 200 OK con la lista completa.
            return Ok(_asignaciones);
        }

        [HttpGet("{id}")] // GET: api/asignaciones/5
        public ActionResult<Asignacion> GetById(int id)
        {
            var asignacion = _asignaciones.FirstOrDefault(a => a.Id == id);

            if (asignacion == null)
            {
                // Retornar 404 si no se encontró
                return NotFound();
            }

            return Ok(asignacion);
        }

        [HttpPost] // POST: api/asignaciones
        public ActionResult<Asignacion> Create(Asignacion asignacion)
        {
            int newId = _asignaciones.Any() ? _asignaciones.Max(a => a.Id) + 1 : 1;
            asignacion.Id = newId;

            _asignaciones.Add(asignacion);

            // Devolver respuesta 201 Created con el recurso creado
            return CreatedAtAction(
                nameof(GetById),
                new { id = asignacion.Id },
                asignacion
            );
        }

        [HttpPut("{id}")] // PUT: api/asignaciones/5
        public IActionResult Update(int id, Asignacion asignacion)
        {
            var existing = _asignaciones.FirstOrDefault(a => a.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            // Actualizar propiedades (excepto el Id)
            existing.ServicioId = asignacion.ServicioId;
            existing.EmpleadoId = asignacion.EmpleadoId;
            existing.AssignmentDate = asignacion.AssignmentDate;
            existing.Status = asignacion.Status;

            // Retornar 204 NoContent indicando que se realizó la operación sin devolver cuerpo.
            return NoContent();
        }

        [HttpDelete("{id}")] // DELETE: api/asignaciones/5
        public IActionResult Delete(int id)
        {
            var existing = _asignaciones.FirstOrDefault(a => a.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            _asignaciones.Remove(existing);

            // Retornamos 204 NoContent para indicar que se eliminó correctamente.
            return NoContent();
        }
    }
}
