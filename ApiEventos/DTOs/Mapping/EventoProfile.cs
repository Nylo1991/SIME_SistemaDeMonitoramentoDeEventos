using AutoMapper;
using Shared;
using ApiEventos.DTOs.Request;
using ApiEventos.DTOs.Response;

namespace ApiEventos.Mappings
{
    public class EventoProfile : Profile
    {
        public EventoProfile()
        {
            // ENTITY -> RESPONSE DTO
            CreateMap<Eventos, EventoDto>();

            // REQUEST DTO -> ENTITY
            CreateMap<EventoRequestDto, Eventos>()
                .ForMember(dest => dest.DataHora,
                           opt => opt.MapFrom(_ => DateTime.Now));
        }
    }
}