using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class AppointmentMapperProfile : Profile
    {
        AppointmentMapperProfile()
        {
            CreateMap<Appointment, AppointmentViewModel>().ReverseMap();
        }
    }
}
