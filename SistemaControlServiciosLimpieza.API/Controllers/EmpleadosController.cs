using SistemaControlServiciosLimpieza.Domain.Entities;
using SistemaControlServiciosLimpieza.Infrastructure.Repositories;
using SistemaControlServiciosLimpieza.Api.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/empleados")]
    public class EmpleadosController : ControllerBase
    {
        //private readonly ApplicationDbContext _context; // Contexto de BD para acceder a las tablas
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EmpleadosController(/*ApplicationDbContext context,*/ UnitOfWork unitOfWork, IMapper mapper)
        {
            //_context = context;
            _unitOfWork = unitOfWork;
            // Nota: No inicializamos lista estática, los datos están en la BD.
            _mapper = mapper;
        }

        // GET: api/empleados
        [HttpGet]
        public ActionResult<List<EmpleadoDto>> GetAll()
        {
            // Recupera los empleados (entidades) de la base de datos
            var empleados = _unitOfWork.Empleados.GetAll();

            //// Mapeo manual de entidades Empleado a DTOs EmpleadoDto
            //var empleadoDtos = empleados.Select(e => new EmpleadoDto
            //{
            //    Id = e.Id,
            //    Name = e.Name,
            //    Phone = e.Phone,
            //    Email = e.Email,
            //    Position = e.Position,
            //    IsActive = e.IsActive
            //}).ToList();

            var empleadoDtos = _mapper.Map<List<EmpleadoDto>>(empleados);

            return Ok(empleadoDtos);
        }

        // GET: api/empleados/{id}
        [HttpGet("{id}")]
        public ActionResult<EmpleadoDto> GetById(int id)
        {
            var empleado = _unitOfWork.Empleados.GetById(id);

            if (empleado == null)
                return NotFound();

            //// Mapeo de la entidad encontrada a DTO
            //var empleadoDto = new EmpleadoDto
            //{
            //    Id = empleado.Id,
            //    Name = empleado.Name,
            //    Phone = empleado.Phone,
            //    Email = empleado.Email,
            //    Position = empleado.Position,
            //    IsActive = empleado.IsActive
            //};

            var empleadoDto = _mapper.Map<EmpleadoDto>(empleado);

            return Ok(empleadoDto);
        }

        // POST: api/empleados
        [HttpPost]
        public ActionResult<Empleado> Create([FromBody] CreateEmpleadoDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("El nombre del empleado es obligatorio.");

            //// Mapeo manual de CreateEmpleadoDto a Empleado
            //var empleado = new Empleado
            //{
            //    Name = request.Name,
            //    Phone = request.Phone,
            //    Email = request.Email,
            //    Position = request.Position,
            //    IsActive = true
            //};

            var empleado = _mapper.Map<Empleado>(request);
            empleado.IsActive = true;

            // El UnitOfWork guarda los cambios y la BD genera el Id
            _unitOfWork.Empleados.Create(empleado);
            _unitOfWork.Complete();

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

            //// Mapeo manual de los datos del DTO a la entidad existente
            //existingEmpleado.Name = request.Name;
            //existingEmpleado.Phone = request.Phone;
            //existingEmpleado.Email = request.Email;
            //existingEmpleado.Position = request.Position;
            //existingEmpleado.IsActive = request.IsActive;

            var empleado = _mapper.Map<Empleado>(request);

            // El repositorio devuelve false si el empleado no existe
            if (!_unitOfWork.Empleados.Update(id, empleado))
                return NotFound();

            _unitOfWork.Complete();

            return NoContent();
        }

        // DELETE: api/empleados/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var empleado = _unitOfWork.Empleados.GetById(id);

            if (empleado == null)
                return NotFound();

            _unitOfWork.Empleados.Delete(empleado);
            _unitOfWork.Complete();

            return NoContent();
        }
    }
}