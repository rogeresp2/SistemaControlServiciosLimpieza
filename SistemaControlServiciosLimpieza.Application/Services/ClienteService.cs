using AutoMapper;
using SistemaControlServiciosLimpieza.Application.Dtos;
using SistemaControlServiciosLimpieza.Application.Models;
using SistemaControlServiciosLimpieza.Domain.Entities;
using SistemaControlServiciosLimpieza.Infrastructure.Repositories;
using System.Net.Mail;

namespace SistemaControlServiciosLimpieza.Application.Services
{
    public class ClienteService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClienteService(UnitOfWork unitOfWork, IMapper mapper)
        {
            //_repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public ServiceResult<List<ClienteDto>> GetAllCliente()
        {
            var clientes = _unitOfWork.Clientes.GetAll();

            var clienteDtos = _mapper.Map<List<ClienteDto>>(clientes);
            return ServiceResult<List<ClienteDto>>.SuccessResponse(clienteDtos);
        }

        public ServiceResult<ClienteDto> GetClienteById(int id)
        {
            var cliente = _unitOfWork.Clientes.GetById(id);

            if (cliente == null)
                return ServiceResult<ClienteDto>.ErrorResponse("Cliente no encontrado");

            var clienteDto = _mapper.Map<ClienteDto>(cliente);

            return ServiceResult<ClienteDto>.SuccessResponse(clienteDto);
        }


        public ServiceResult<int> CreateCliente(CreateClienteDto request)
        {
            // Validación básica de negocio
            var error = ValidateCliente(request.Name, request.Phone, request.Email);
            if (error != null)
                return ServiceResult<int>.ErrorResponse(error);

            var cliente = _mapper.Map<Cliente>(request);
            cliente.IsActive = true;

            // El UnitOfWork guarda los cambios y la BD genera el Id
            _unitOfWork.Clientes.Create(cliente);
            _unitOfWork.Complete();

            return ServiceResult<int>.SuccessResponse(cliente.Id, "Cliente creado correctamente.");
        }


        public ServiceResult<bool> UpdateCliente(int id, ClienteDto request)
        {
            if (id != request.Id)
            {
                return ServiceResult<bool>.ErrorResponse("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");
            }

            var error = ValidateCliente(request.Name, request.Phone, request.Email);
            if (error != null)
                return ServiceResult<bool>.ErrorResponse(error);

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return ServiceResult<bool>.ErrorResponse("El nombre del cliente no puede estar vacío.");
            }

            var cliente = _mapper.Map<Cliente>(request);

            // El repositorio devuelve false si el cliente no existe
            if (!_unitOfWork.Clientes.Update(id, cliente))
            {
                return ServiceResult<bool>.ErrorResponse("Cliente no encontrado");
            }
            _unitOfWork.Complete();

            return ServiceResult<bool>.SuccessResponse(true, "Cliente actualizado correctamente.");
        }


        public ServiceResult<bool> DeleteCliente(int id)
        {
            var cliente = _unitOfWork.Clientes.GetById(id);

            if (cliente == null)
            {
                return ServiceResult<bool>.ErrorResponse("Cliente no encontrado");
            }

            _unitOfWork.Clientes.Delete(cliente);
            _unitOfWork.Complete();

            return ServiceResult<bool>.SuccessResponse(true, "Cliente eliminado correctamente.");
        }

        // Validaciones de los campos del cliente. Devuelve el mensaje de error, o null si todo es válido.
        private string? ValidateCliente(string name, string? phone, string? email)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "El nombre del cliente es obligatorio.";

            if (name.Length > 100)
                return "El nombre del cliente no puede tener más de 100 caracteres.";

            if (phone != null && phone.Length > 15)
                return "El teléfono no puede tener más de 15 caracteres.";

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (email.Length > 150)
                    return "El correo no puede tener más de 150 caracteres.";

                if (!MailAddress.TryCreate(email, out _))
                    return "El correo no tiene un formato válido.";
            }

            return null;
        }

    }
}