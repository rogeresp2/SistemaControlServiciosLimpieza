using Microsoft.AspNetCore.Mvc;
using SistemaControlServiciosLimpieza.Application.Dtos;
using SistemaControlServiciosLimpieza.Application.Models;
using SistemaControlServiciosLimpieza.Application.Services;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/asignaciones")]
    public class AsignacionesController : ControllerBase
    {
        //private readonly ApplicationDbContext _context; // Contexto de BD para acceder a las tablas
        private readonly AsignacionService _asignacionService;

        public AsignacionesController(/*ApplicationDbContext context,*/ AsignacionService asignacionService)
        {
            //_context = context;
            _asignacionService = asignacionService;
            // Nota: No inicializamos lista estática, los datos están en la BD.
        }

        // GET: api/asignaciones
        [HttpGet]
        public ActionResult<ApiResponse<List<AsignacionDto>>> GetAll()
        {
            // Recupera las asignaciones (entidades) de la base de datos
            // Mapeo de entidades Asignacion a DTOs AsignacionDto utilizando AutoMapper
            var result = _asignacionService.GetAllAsignacion();

            return ToApiResponse(result);
        }

        // GET: api/asignaciones/{id}
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<AsignacionDto>> GetById(int id)
        {
            // Mapeo de la entidad encontrada a DTO utilizando AutoMapper
            var result = _asignacionService.GetAsignacionById(id);

            return ToApiResponse(result, 200, 404);
        }

        // GET /api/asignaciones/con-detalles
        [HttpGet("con-detalles")]
        public ActionResult<ApiResponse<List<AsignacionWithDetailsDto>>> GetAsignacionesWithDetails()
        {
            // Mapeo de entidades Asignacion a DTOs AsignacionWithDetailsDto utilizando AutoMapper
            var result = _asignacionService.GetAllAsignacionWithDetails();

            return ToApiResponse(result);
        }

        // POST: api/asignaciones
        [HttpPost]
        public ActionResult<ApiResponse<int>> Create([FromBody] CreateAsignacionDto request)
        {
            // Mapeo de CreateAsignacionDto a Asignacion utilizando AutoMapper
            // AsignacionService valida, guarda los cambios y la BD genera el Id
            var result = _asignacionService.CreateAsignacion(request);

            return ToApiResponse(result, 201);
        }

        // PUT: api/asignaciones/5
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<bool>> Update(int id, [FromBody] AsignacionDto request)
        {
            // Mapeo de los datos del DTO a la entidad utilizando AutoMapper
            // AsignacionService devuelve error si la asignación no existe
            var result = _asignacionService.UpdateAsignacion(id, request);

            return ToApiResponse(result);
        }

        // DELETE: api/asignaciones/5
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<bool>> Delete(int id)
        {
            var result = _asignacionService.DeleteAsignacion(id);

            return ToApiResponse(result, 200, 404);
        }

        // Convierte el resultado del servicio en la respuesta del API, agregando el código HTTP
        private ActionResult<ApiResponse<T>> ToApiResponse<T>(ServiceResult<T> result, int successCode = 200, int errorCode = 400)
        {
            if (result.Success)
                return StatusCode(successCode, ApiResponse<T>.SuccessResponse(result.Data, successCode, result.Message));

            return StatusCode(errorCode, ApiResponse<T>.ErrorResponse(result.Message, errorCode));
        }
    }
}