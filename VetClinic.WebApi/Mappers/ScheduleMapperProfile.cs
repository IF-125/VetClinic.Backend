using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class ScheduleMapperProfile : Profile
    {
        public ScheduleMapperProfile()
        {
            CreateMap<Schedule, ScheduleViewModel>()
                .ForMember(x => x.Day, y => y.MapFrom(src => src.Day))
                .ReverseMap();
        }
    }
}
