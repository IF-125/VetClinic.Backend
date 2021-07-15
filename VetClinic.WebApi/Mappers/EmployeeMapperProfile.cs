using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class EmployeeMapperProfile : Profile
    {
        public EmployeeMapperProfile()
        {
            CreateMap<Employee, EmployeeViewModel>()
                
                .ForMember(x => x.FirstName, y => y.MapFrom(src => src.FirstName))

                .ForMember(x => x.LastName, y => y.MapFrom(src => src.LastName))

                .ReverseMap();
        }
    }
}
