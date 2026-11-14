using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaControlServiciosLimpieza.Api.Models.Dtos;
using SistemaControlServiciosLimpieza.Domain.Entities;
using SistemaControlServiciosLimpieza.Infrastructure.Repositories;

namespace SistemaControlServiciosLimpieza.Api.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        //private readonly ClienteRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientesController(/*ClienteRepository repository,*/ UnitOfWork unitOfWork, IMapper mapper)
        {
            //_repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/clientes
        [HttpGet]
        public ActionResult<List<ClienteDto>> GetAll()
        {
            var clientes = _unitOfWork.Clientes.GetAll();

            var clienteDtos = _mapper.Map<List<ClienteDto>>(clientes);

            return Ok(clienteDtos);
        }

        // GET: api/clientes/{id}
        [HttpGet("{id}")]
        public ActionResult<ClienteDto> GetById(int id)
        {
            var cliente = _unitOfWork.Clientes.GetById(id);

            if (cliente == null)
                return NotFound();

            var clienteDto = _mapper.Map<ClienteDto>(cliente);

            return Ok(clienteDto);
        }

        // POST: api/clientes
        [HttpPost]
        public ActionResult<Cliente> Create([FromBody] CreateClienteDto request)
        {
            // Validación básica de negocio
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("El nombre del cliente es obligatorio.");
            }

            var cliente = _mapper.Map<Cliente>(request);
            cliente.IsActive = true;

            // El UnitOfWork guarda los cambios y la BD genera el Id
            _unitOfWork.Clientes.Create(cliente);
            _unitOfWork.Complete();

            return Ok(new { cliente.Id });
        }

        // PUT: api/clientes/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ClienteDto request)
        {
            if (id != request.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("El nombre del cliente no puede estar vacío.");
            }

            var cliente = _mapper.Map<Cliente>(request);

            // El repositorio devuelve false si el cliente no existe
            if (!_unitOfWork.Clientes.Update(id, cliente))
            {
                return NotFound();
            }
            _unitOfWork.Complete();

            return NoContent();
        }

        // DELETE: api/clientes/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cliente = _unitOfWork.Clientes.GetById(id);

            if (cliente == null)
            {
                return NotFound();
            }

            _unitOfWork.Clientes.Delete(cliente);
            _unitOfWork.Complete();

            return NoContent();
        }
    }
}