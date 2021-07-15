using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class SalaryMapperProfile : Profile
    {
        public SalaryMapperProfile()
        {
            CreateMap<Salary, SalaryViewModel>();
        }
    }
}
