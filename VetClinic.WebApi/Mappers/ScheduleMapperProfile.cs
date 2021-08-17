using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.Converters;
using VetClinic.WebApi.ViewModels.ScheduleViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class ScheduleMapperProfile : Profile
    {
        public ScheduleMapperProfile()
        {
            CreateMap<Schedule, ScheduleViewModel>()

                .ForMember(x => x.From, opt => opt.ConvertUsing(new StringToTimeSpanConverter()))
                
                .ForMember(x => x.To, opt => opt.ConvertUsing(new StringToTimeSpanConverter()))

                .ReverseMap();
        }
    }
}
