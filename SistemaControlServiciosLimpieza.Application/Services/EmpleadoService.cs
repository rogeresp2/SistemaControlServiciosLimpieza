using AutoMapper;
using SistemaControlServiciosLimpieza.Application.Dtos;
using SistemaControlServiciosLimpieza.Application.Models;
using SistemaControlServiciosLimpieza.Domain.Entities;
using SistemaControlServiciosLimpieza.Infrastructure.Repositories;
using System.Net.Mail;

namespace SistemaControlServiciosLimpieza.Application.Services
{
    public class EmpleadoService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmpleadoService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ServiceResult<List<EmpleadoDto>> GetAllEmpleado()
        {
            var empleados = _unitOfWork.Empleados.GetAll();

            var empleadoDtos = _mapper.Map<List<EmpleadoDto>>(empleados);
            return ServiceResult<List<EmpleadoDto>>.SuccessResponse(empleadoDtos);
        }

        public ServiceResult<EmpleadoDto> GetEmpleadoById(int id)
        {
            var empleado = _unitOfWork.Empleados.GetById(id);

            if (empleado == null)
                return ServiceResult<EmpleadoDto>.ErrorResponse("Empleado no encontrado");

            var empleadoDto = _mapper.Map<EmpleadoDto>(empleado);

            return ServiceResult<EmpleadoDto>.SuccessResponse(empleadoDto);
        }

        public ServiceResult<int> CreateEmpleado(CreateEmpleadoDto request)
        {
            var error = ValidateEmpleado(request.Name, request.Phone, request.Email, request.Position);
            if (error != null)
                return ServiceResult<int>.ErrorResponse(error);

            var empleado = _mapper.Map<Empleado>(request);
            empleado.IsActive = true;

            // El UnitOfWork guarda los cambios y la BD genera el Id
            _unitOfWork.Empleados.Create(empleado);
            _unitOfWork.Complete();

            return ServiceResult<int>.SuccessResponse(empleado.Id, "Empleado creado correctamente.");
        }

        public ServiceResult<bool> UpdateEmpleado(int id, EmpleadoDto request)
        {
            if (id != request.Id)
                return ServiceResult<bool>.ErrorResponse("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");

            var error = ValidateEmpleado(request.Name, request.Phone, request.Email, request.Position);
            if (error != null)
                return ServiceResult<bool>.ErrorResponse(error);

            var empleado = _mapper.Map<Empleado>(request);

            // El repositorio devuelve false si el empleado no existe
            if (!_unitOfWork.Empleados.Update(id, empleado))
                return ServiceResult<bool>.ErrorResponse("Empleado no encontrado");

            _unitOfWork.Complete();

            return ServiceResult<bool>.SuccessResponse(true, "Empleado actualizado correctamente.");
        }

        public ServiceResult<bool> DeleteEmpleado(int id)
        {
            var empleado = _unitOfWork.Empleados.GetById(id);

            if (empleado == null)
                return ServiceResult<bool>.ErrorResponse("Empleado no encontrado");

            _unitOfWork.Empleados.Delete(empleado);
            _unitOfWork.Complete();

            return ServiceResult<bool>.SuccessResponse(true, "Empleado eliminado correctamente.");
        }

        // Validaciones de los campos del empleado. Devuelve el mensaje de error, o null si todo es válido.
        private string? ValidateEmpleado(string name, string? phone, string? email, string? position)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "El nombre del empleado es obligatorio.";

            if (name.Length > 100)
                return "El nombre del empleado no puede tener más de 100 caracteres.";

            if (phone != null && phone.Length > 15)
                return "El teléfono no puede tener más de 15 caracteres.";

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (email.Length > 150)
                    return "El correo no puede tener más de 150 caracteres.";

                if (!MailAddress.TryCreate(email, out _))
                    return "El correo no tiene un formato válido.";
            }

            if (position != null && position.Length > 50)
                return "El cargo no puede tener más de 50 caracteres.";

            return null;
        }
    }
}