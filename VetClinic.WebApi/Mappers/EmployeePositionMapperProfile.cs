using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class EmployeePositionMapperProfile : Profile
    {
        public EmployeePositionMapperProfile()
        {
            CreateMap<EmployeePosition, EmployeePositionViewModel>()
                .ForMember(x => x.EmployeeFullName,
                    y => y.MapFrom(src => src.Employee.ToString()))

                .ForMember(x => x.Address,
                    y => y.MapFrom(src => src.Employee.Address))

                .ForMember(x => x.PositionName,
                    y => y.MapFrom(src => src.Position.Title))
                
                .ReverseMap();
        }
    }
}
