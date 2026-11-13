using AutoMapper;
using SistemaControlServiciosLimpieza.Api.Models.Dtos;
using SistemaControlServiciosLimpieza.Api.Models.Entities;

namespace SistemaControlServiciosLimpieza.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cliente
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<CreateClienteDto, Cliente>();

            // Empleado
            CreateMap<Empleado, EmpleadoDto>().ReverseMap();
            CreateMap<CreateEmpleadoDto, Empleado>();

            // Servicio
            CreateMap<Servicio, ServicioDto>().ReverseMap();
            CreateMap<CreateServicioDto, Servicio>();

            // Servicio con Cliente
            CreateMap<Servicio, ServicioWithClienteDto>()
                .ForMember(dest => dest.ClienteName, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Name : "")
                );

            // Asignacion
            CreateMap<Asignacion, AsignacionDto>().ReverseMap();
            CreateMap<CreateAsignacionDto, Asignacion>();

            // Asignacion con detalles
            CreateMap<Asignacion, AsignacionWithDetailsDto>()
                .ForMember(dest => dest.ServicioName, opt => opt.MapFrom(src => src.Servicio != null ? src.Servicio.Name : "")
                )
                .ForMember(dest => dest.EmpleadoName, opt => opt.MapFrom(src => src.Empleado != null ? src.Empleado.Name : "")
                );
        }
    }
}
