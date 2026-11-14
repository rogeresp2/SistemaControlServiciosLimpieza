using SistemaControlServiciosLimpieza.Domain.Entities;
using SistemaControlServiciosLimpieza.Infrastructure.Repositories;
using SistemaControlServiciosLimpieza.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/asignaciones")]
    public class AsignacionesController : ControllerBase
    {
        //private readonly ApplicationDbContext _context; // Contexto de BD para acceder a las tablas
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AsignacionesController(/*ApplicationDbContext context,*/ UnitOfWork unitOfWork, IMapper mapper)
        {
            //_context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            // Nota: No inicializamos lista estática, los datos están en la BD.
        }

        // GET: api/asignaciones
        [HttpGet]
        public ActionResult<List<AsignacionDto>> GetAll()
        {
            // Recupera las asignaciones (entidades) de la base de datos
            var asignaciones = _unitOfWork.Asignaciones.GetAll();

            // Mapeo manual de entidades Asignacion a DTOs AsignacionDto
            // var asignacionDtos = asignaciones.Select(a => new AsignacionDto
            // {
            //     Id = a.Id,
            //     ServicioId = a.ServicioId,
            //     EmpleadoId = a.EmpleadoId,
            //     AssignmentDate = a.AssignmentDate,
            //     Status = a.Status
            // }).ToList();

            // Mapeo de entidades Asignacion a DTOs AsignacionDto utilizando AutoMapper
            var asignacionDtos = _mapper.Map<List<AsignacionDto>>(asignaciones);

            return Ok(asignacionDtos);
        }

        // GET: api/asignaciones/{id}
        [HttpGet("{id}")]
        public ActionResult<AsignacionDto> GetById(int id)
        {
            var asignacion = _unitOfWork.Asignaciones.GetById(id);

            if (asignacion == null)
                return NotFound();

            // Mapeo de la entidad encontrada a DTO
            // var asignacionDto = new AsignacionDto
            // {
            //     Id = asignacion.Id,
            //     ServicioId = asignacion.ServicioId,
            //     EmpleadoId = asignacion.EmpleadoId,
            //     AssignmentDate = asignacion.AssignmentDate,
            //     Status = asignacion.Status
            // };

            // Mapeo de la entidad encontrada a DTO utilizando AutoMapper
            var asignacionDto = _mapper.Map<AsignacionDto>(asignacion);

            return Ok(asignacionDto);
        }

        // GET /api/asignaciones/con-detalles
        [HttpGet("con-detalles")]
        public ActionResult<List<AsignacionWithDetailsDto>> GetAsignacionesWithDetails()
        {
            var asignaciones = _unitOfWork.Asignaciones.GetAllWithDetails();

            // Mapeo manual de entidades Asignacion a DTOs AsignacionWithDetailsDto
            // var asignacionDTOs = asignaciones.Select(a => new AsignacionWithDetailsDto
            // {
            //     Id = a.Id,
            //     ServicioId = a.ServicioId,
            //     ServicioName = a.Servicio != null ? a.Servicio.Name : "",
            //     EmpleadoId = a.EmpleadoId,
            //     EmpleadoName = a.Empleado != null ? a.Empleado.Name : "",
            //     AssignmentDate = a.AssignmentDate,
            //     Status = a.Status
            // }).ToList();

            // Mapeo de entidades Asignacion a DTOs AsignacionWithDetailsDto utilizando AutoMapper
            var asignacionDTOs = _mapper.Map<List<AsignacionWithDetailsDto>>(asignaciones);

            return Ok(asignacionDTOs);
        }

        // POST: api/asignaciones
        [HttpPost]
        public ActionResult<Asignacion> Create([FromBody] CreateAsignacionDto request)
        {
            if (!_unitOfWork.Servicios.Exists(request.ServicioId))
                return BadRequest("El servicio indicado no existe.");

            if (!_unitOfWork.Empleados.Exists(request.EmpleadoId))
                return BadRequest("El empleado indicado no existe.");

            // Mapeo manual de CreateAsignacionDto a Asignacion
            // var asignacion = new Asignacion
            // {
            //     ServicioId = request.ServicioId,
            //     EmpleadoId = request.EmpleadoId,
            //     AssignmentDate = request.AssignmentDate,
            //     Status = request.Status
            // };

            // Mapeo de CreateAsignacionDto a Asignacion utilizando AutoMapper
            var asignacion = _mapper.Map<Asignacion>(request);

            // El UnitOfWork guarda los cambios y la BD genera el Id
            _unitOfWork.Asignaciones.Create(asignacion);
            _unitOfWork.Complete();

            return Ok(new { asignacion.Id });
        }

        // PUT: api/asignaciones/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AsignacionDto request)
        {
            if (id != request.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");

            if (!_unitOfWork.Servicios.Exists(request.ServicioId))
                return BadRequest("El servicio indicado no existe.");

            if (!_unitOfWork.Empleados.Exists(request.EmpleadoId))
                return BadRequest("El empleado indicado no existe.");

            // Mapeo manual de los datos del DTO a la entidad existente
            // existingAsignacion.ServicioId = request.ServicioId;
            // existingAsignacion.EmpleadoId = request.EmpleadoId;
            // existingAsignacion.AssignmentDate = request.AssignmentDate;
            // existingAsignacion.Status = request.Status;

            // Mapeo de los datos del DTO a la entidad utilizando AutoMapper
            var asignacion = _mapper.Map<Asignacion>(request);

            // El repositorio devuelve false si la asignación no existe
            if (!_unitOfWork.Asignaciones.Update(id, asignacion))
                return NotFound();

            _unitOfWork.Complete();

            return NoContent();
        }

        // DELETE: api/asignaciones/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var asignacion = _unitOfWork.Asignaciones.GetById(id);

            if (asignacion == null)
                return NotFound();

            _unitOfWork.Asignaciones.Delete(asignacion);
            _unitOfWork.Complete();

            return NoContent();
        }
    }
}