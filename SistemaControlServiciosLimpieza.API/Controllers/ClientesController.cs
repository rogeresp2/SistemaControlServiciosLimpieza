using Microsoft.AspNetCore.Mvc;
using SistemaControlServiciosLimpieza.Application.Dtos;
using SistemaControlServiciosLimpieza.Application.Models;
using SistemaControlServiciosLimpieza.Application.Services;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClientesController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/clientes
        [HttpGet]
        public ActionResult<ApiResponse<List<ClienteDto>>> GetAll()
        {
            var result = _clienteService.GetAllCliente();
            return ToApiResponse(result);
        }

        // GET: api/clientes/{id}
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<ClienteDto>> GetById(int id)
        {
            var result = _clienteService.GetClienteById(id);
            return ToApiResponse(result, 200, 404);
        }

        // POST: api/clientes
        [HttpPost]
        public ActionResult<ApiResponse<int>> Create([FromBody] CreateClienteDto request)
        {
            var result = _clienteService.CreateCliente(request);
            return ToApiResponse(result, 201);
        }

        // PUT: api/clientes/5
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<bool>> Update(int id, [FromBody] ClienteDto request)
        {
            var result = _clienteService.UpdateCliente(id, request);
            return ToApiResponse(result);
        }

        // DELETE: api/clientes/5
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<bool>> Delete(int id)
        {
            var result = _clienteService.DeleteCliente(id);
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