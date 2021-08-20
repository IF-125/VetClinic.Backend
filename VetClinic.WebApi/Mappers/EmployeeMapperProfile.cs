using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels.EmployeeViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class EmployeeMapperProfile : Profile
    {
        public EmployeeMapperProfile()
        {
            CreateMap<Employee, EmployeeViewModel>()
                
                .ForMember(x => x.FirstName, y => y.MapFrom(src => src.FirstName))

                .ForMember(x => x.LastName, y => y.MapFrom(src => src.LastName))

                .ForMember(x => x.Position, y => y.MapFrom(src => src.EmployeePosition.Position.Title))

                .ReverseMap();
        }
    }
}
