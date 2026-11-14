using Microsoft.AspNetCore.Mvc;
using SistemaControlServiciosLimpieza.Application.Dtos;
using SistemaControlServiciosLimpieza.Application.Models;
using SistemaControlServiciosLimpieza.Application.Services;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/empleados")]
    public class EmpleadosController : ControllerBase
    {
        //private readonly ApplicationDbContext _context; // Contexto de BD para acceder a las tablas
        private readonly EmpleadoService _empleadoService;
        public EmpleadosController(/*ApplicationDbContext context,*/ EmpleadoService empleadoService)
        {
            //_context = context;
            _empleadoService = empleadoService;
            // Nota: No inicializamos lista estática, los datos están en la BD.
        }

        // GET: api/empleados
        [HttpGet]
        public ApiResponse<List<EmpleadoDto>> GetAll()
        {
            // Recupera los empleados (entidades) de la base de datos
            var result = _empleadoService.GetAllEmpleado();

            return ToApiResponse(result);
        }

        // GET: api/empleados/{id}
        [HttpGet("{id}")]
        public ApiResponse<EmpleadoDto> GetById(int id)
        {
            var result = _empleadoService.GetEmpleadoById(id);

            return ToApiResponse(result, 200, 404);
        }

        // POST: api/empleados
        [HttpPost]
        public ApiResponse<int> Create([FromBody] CreateEmpleadoDto request)
        {
            // El servicio valida, guarda los cambios y la BD genera el Id
            var result = _empleadoService.CreateEmpleado(request);

            return ToApiResponse(result, 201);
        }

        // PUT: api/empleados/5
        [HttpPut("{id}")]
        public ApiResponse<bool> Update(int id, [FromBody] EmpleadoDto request)
        {
            // El servicio devuelve error si el empleado no existe
            var result = _empleadoService.UpdateEmpleado(id, request);

            return ToApiResponse(result);
        }

        // DELETE: api/empleados/5
        [HttpDelete("{id}")]
        public ApiResponse<bool> Delete(int id)
        {
            var result = _empleadoService.DeleteEmpleado(id);

            return ToApiResponse(result, 200, 404);
        }

        // Convierte el resultado del servicio en la respuesta del API, agregando el código HTTP
        private static ApiResponse<T> ToApiResponse<T>(ServiceResult<T> result, int successCode = 200, int errorCode = 400)
        {
            if (result.Success)
                return ApiResponse<T>.SuccessResponse(result.Data, successCode, result.Message);

            return ApiResponse<T>.ErrorResponse(result.Message, errorCode);
        }
    }
}