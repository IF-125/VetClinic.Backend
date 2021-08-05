using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class AppointmentMapperProfile : Profile
    {
        public AppointmentMapperProfile()
        {
            CreateMap<Appointment, AppointmentViewModel>()

                .ForMember(x => x.Date, y => y.MapFrom(src => src.From.ToString("d")))

                .ForMember(x => x.From, y => y.MapFrom(src => src.From.ToString("HH:mm")))

                .ForMember(x => x.To, y => y.MapFrom(src => src.To.ToString("HH:mm")))
                
                .ReverseMap();
        }
    }
}
