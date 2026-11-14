using AutoMapper;
using SistemaControlServiciosLimpieza.Application.Dtos;
using SistemaControlServiciosLimpieza.Application.Models;
using SistemaControlServiciosLimpieza.Domain.Entities;
using SistemaControlServiciosLimpieza.Infrastructure.Repositories;

namespace SistemaControlServiciosLimpieza.Application.Services
{
    public class ServicioService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServicioService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ServiceResult<List<ServicioDto>> GetAllServicio()
        {
            var servicios = _unitOfWork.Servicios.GetAll();

            var servicioDtos = _mapper.Map<List<ServicioDto>>(servicios);
            return ServiceResult<List<ServicioDto>>.SuccessResponse(servicioDtos);
        }

        public ServiceResult<ServicioDto> GetServicioById(int id)
        {
            var servicio = _unitOfWork.Servicios.GetById(id);

            if (servicio == null)
                return ServiceResult<ServicioDto>.ErrorResponse("Servicio no encontrado");

            var servicioDto = _mapper.Map<ServicioDto>(servicio);

            return ServiceResult<ServicioDto>.SuccessResponse(servicioDto);
        }

        public ServiceResult<List<ServicioWithClienteDto>> GetAllServicioWithCliente()
        {
            var servicios = _unitOfWork.Servicios.GetAllWithCliente();

            var servicioDtos = _mapper.Map<List<ServicioWithClienteDto>>(servicios);
            return ServiceResult<List<ServicioWithClienteDto>>.SuccessResponse(servicioDtos);
        }

        public ServiceResult<int> CreateServicio(CreateServicioDto request)
        {
            var error = ValidateServicio(request.Name, request.ServiceDate, request.Cost, request.Status, request.ClienteId);
            if (error != null)
                return ServiceResult<int>.ErrorResponse(error);

            var servicio = _mapper.Map<Servicio>(request);

            // El UnitOfWork guarda los cambios y la BD genera el Id
            _unitOfWork.Servicios.Create(servicio);
            _unitOfWork.Complete();

            return ServiceResult<int>.SuccessResponse(servicio.Id, "Servicio creado correctamente.");
        }

        public ServiceResult<bool> UpdateServicio(int id, ServicioDto request)
        {
            if (id != request.Id)
                return ServiceResult<bool>.ErrorResponse("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");

            var error = ValidateServicio(request.Name, request.ServiceDate, request.Cost, request.Status, request.ClienteId);
            if (error != null)
                return ServiceResult<bool>.ErrorResponse(error);

            var servicio = _mapper.Map<Servicio>(request);

            // El repositorio devuelve false si el servicio no existe
            if (!_unitOfWork.Servicios.Update(id, servicio))
                return ServiceResult<bool>.ErrorResponse("Servicio no encontrado");

            _unitOfWork.Complete();

            return ServiceResult<bool>.SuccessResponse(true, "Servicio actualizado correctamente.");
        }

        public ServiceResult<bool> DeleteServicio(int id)
        {
            var servicio = _unitOfWork.Servicios.GetById(id);

            if (servicio == null)
                return ServiceResult<bool>.ErrorResponse("Servicio no encontrado");

            _unitOfWork.Servicios.Delete(servicio);
            _unitOfWork.Complete();

            return ServiceResult<bool>.SuccessResponse(true, "Servicio eliminado correctamente.");
        }

        // Validaciones de los campos del servicio. Devuelve el mensaje de error, o null si todo es válido.
        private string? ValidateServicio(string name, DateTime serviceDate, decimal cost, string status, int clienteId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "El nombre del servicio es obligatorio.";

            if (name.Length > 100)
                return "El nombre del servicio no puede tener más de 100 caracteres.";

            if (serviceDate == default)
                return "La fecha del servicio es obligatoria.";

            if (cost < 0)
                return "El costo del servicio no puede ser negativo.";

            if (string.IsNullOrWhiteSpace(status))
                return "El estado del servicio es obligatorio.";

            if (status.Length > 30)
                return "El estado del servicio no puede tener más de 30 caracteres.";

            if (!_unitOfWork.Clientes.Exists(clienteId))
                return "El cliente indicado no existe.";

            return null;
        }
    }
}