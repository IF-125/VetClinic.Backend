using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels.AppointmentViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class AppointmentMapperProfile : Profile
    {
        public AppointmentMapperProfile()
        {
            #region AppointmentViewModel mapping
            CreateMap<Appointment, AppointmentViewModel>()

                .ForMember(x => x.From, y => y.MapFrom(src => src.From.ToString("HH:mm")))

                .ForMember(x => x.To, y => y.MapFrom(src => src.To.ToString("HH:mm")))

                .ForMember(x => x.OrderProcedureId, y => y.MapFrom(src => src.OrderProcedure.Id))

                .ForMember(x => x.DoctorName, y => y.MapFrom(src => src.OrderProcedure.Employee.FirstName + " " + src.OrderProcedure.Employee.LastName))

                .ReverseMap();
            #endregion

            #region AppointmentToCreateViewModel mapping
            CreateMap<Appointment, AppointmentToCreateViewModel>()
                .ReverseMap();
            #endregion
        }
    }
}
