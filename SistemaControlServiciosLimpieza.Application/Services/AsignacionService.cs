using AutoMapper;
using SistemaControlServiciosLimpieza.Application.Dtos;
using SistemaControlServiciosLimpieza.Application.Models;
using SistemaControlServiciosLimpieza.Domain.Entities;
using SistemaControlServiciosLimpieza.Infrastructure.Repositories;

namespace SistemaControlServiciosLimpieza.Application.Services
{
    public class AsignacionService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AsignacionService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ServiceResult<List<AsignacionDto>> GetAllAsignacion()
        {
            var asignaciones = _unitOfWork.Asignaciones.GetAll();

            var asignacionDtos = _mapper.Map<List<AsignacionDto>>(asignaciones);
            return ServiceResult<List<AsignacionDto>>.SuccessResponse(asignacionDtos);
        }

        public ServiceResult<AsignacionDto> GetAsignacionById(int id)
        {
            var asignacion = _unitOfWork.Asignaciones.GetById(id);

            if (asignacion == null)
                return ServiceResult<AsignacionDto>.ErrorResponse("Asignación no encontrada");

            var asignacionDto = _mapper.Map<AsignacionDto>(asignacion);

            return ServiceResult<AsignacionDto>.SuccessResponse(asignacionDto);
        }

        public ServiceResult<List<AsignacionWithDetailsDto>> GetAllAsignacionWithDetails()
        {
            var asignaciones = _unitOfWork.Asignaciones.GetAllWithDetails();

            var asignacionDtos = _mapper.Map<List<AsignacionWithDetailsDto>>(asignaciones);
            return ServiceResult<List<AsignacionWithDetailsDto>>.SuccessResponse(asignacionDtos);
        }

        public ServiceResult<int> CreateAsignacion(CreateAsignacionDto request)
        {
            var error = ValidateAsignacion(request.ServicioId, request.EmpleadoId, request.AssignmentDate, request.Status);
            if (error != null)
                return ServiceResult<int>.ErrorResponse(error);

            var asignacion = _mapper.Map<Asignacion>(request);

            // El UnitOfWork guarda los cambios y la BD genera el Id
            _unitOfWork.Asignaciones.Create(asignacion);
            _unitOfWork.Complete();

            return ServiceResult<int>.SuccessResponse(asignacion.Id, "Asignación creada correctamente.");
        }

        public ServiceResult<bool> UpdateAsignacion(int id, AsignacionDto request)
        {
            if (id != request.Id)
                return ServiceResult<bool>.ErrorResponse("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");

            var error = ValidateAsignacion(request.ServicioId, request.EmpleadoId, request.AssignmentDate, request.Status);
            if (error != null)
                return ServiceResult<bool>.ErrorResponse(error);

            var asignacion = _mapper.Map<Asignacion>(request);

            // El repositorio devuelve false si la asignación no existe
            if (!_unitOfWork.Asignaciones.Update(id, asignacion))
                return ServiceResult<bool>.ErrorResponse("Asignación no encontrada");

            _unitOfWork.Complete();

            return ServiceResult<bool>.SuccessResponse(true, "Asignación actualizada correctamente.");
        }

        public ServiceResult<bool> DeleteAsignacion(int id)
        {
            var asignacion = _unitOfWork.Asignaciones.GetById(id);

            if (asignacion == null)
                return ServiceResult<bool>.ErrorResponse("Asignación no encontrada");

            _unitOfWork.Asignaciones.Delete(asignacion);
            _unitOfWork.Complete();

            return ServiceResult<bool>.SuccessResponse(true, "Asignación eliminada correctamente.");
        }

        // Validaciones de los campos de la asignación. Devuelve el mensaje de error, o null si todo es válido.
        private string? ValidateAsignacion(int servicioId, int empleadoId, DateTime assignmentDate, string status)
        {
            if (!_unitOfWork.Servicios.Exists(servicioId))
                return "El servicio indicado no existe.";

            if (!_unitOfWork.Empleados.Exists(empleadoId))
                return "El empleado indicado no existe.";

            if (assignmentDate == default)
                return "La fecha de asignación es obligatoria.";

            if (string.IsNullOrWhiteSpace(status))
                return "El estado de la asignación es obligatorio.";

            if (status.Length > 30)
                return "El estado de la asignación no puede tener más de 30 caracteres.";

            return null;
        }
    }
}