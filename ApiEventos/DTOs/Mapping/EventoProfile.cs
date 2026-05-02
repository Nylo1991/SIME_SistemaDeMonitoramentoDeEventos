using ApiEventos.DTOs;
using ApiEventos.DTOs.Request;
using ApiEventos.DTOs.Response;
using AutoMapper;
using Shared;

namespace ApiEventos.Mappings
{
    public class EventoProfile : Profile
    {
        public EventoProfile()
        {
            CreateMap<Eventos, EventoDto>();

            CreateMap<EventoRequestDto, Eventos>()
                .ForMember(d => d.DataHora, o => o.MapFrom(_ => DateTime.Now));
        }
    }
}