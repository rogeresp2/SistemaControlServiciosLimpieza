using Microsoft.AspNetCore.Mvc;
using SistemaControlServiciosLimpieza.Application.Dtos;
using SistemaControlServiciosLimpieza.Application.Models;
using SistemaControlServiciosLimpieza.Application.Services;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/servicios")]
    public class ServiciosController : ControllerBase
    {
        //private readonly ApplicationDbContext _context; // Contexto de BD para acceder a las tablas
        private readonly ServicioService _servicioService;

        public ServiciosController(/*ApplicationDbContext context,*/ ServicioService servicioService)
        {
            //_context = context;
            _servicioService = servicioService;
            // Nota: No inicializamos lista estática, los datos están en la BD.
        }

        // GET: api/servicios
        [HttpGet]
        public ActionResult<ApiResponse<List<ServicioDto>>> GetAll()
        {
            // Recupera los servicios (entidades) de la base de datos
            var result = _servicioService.GetAllServicio();

            return ToApiResponse(result);
        }

        // GET: api/servicios/{id}
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<ServicioDto>> GetById(int id)
        {
            var result = _servicioService.GetServicioById(id);

            return ToApiResponse(result, 200, 404);
        }

        // GET /api/servicios/con-cliente
        [HttpGet("con-cliente")]
        public ActionResult<ApiResponse<List<ServicioWithClienteDto>>> GetServiciosWithCliente()
        {
            var result = _servicioService.GetAllServicioWithCliente();

            return ToApiResponse(result);
        }

        // POST: api/servicios
        [HttpPost]
        public ActionResult<ApiResponse<int>> Create([FromBody] CreateServicioDto request)
        {
            // ServicioService valida, guarda los cambios y la BD genera el Id
            var result = _servicioService.CreateServicio(request);

            return ToApiResponse(result, 201);
        }

        // PUT: api/servicios/5
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<bool>> Update(int id, [FromBody] ServicioDto request)
        {
            // ServicioService devuelve error si el servicio no existe
            var result = _servicioService.UpdateServicio(id, request);

            return ToApiResponse(result);
        }

        // DELETE: api/servicios/5
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<bool>> Delete(int id)
        {
            var result = _servicioService.DeleteServicio(id);

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