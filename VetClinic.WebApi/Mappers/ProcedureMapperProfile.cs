using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class ProcedureMapperProfile : Profile
    {
        ProcedureMapperProfile()
        {
            CreateMap<Procedure, ProcedureViewModel>().ReverseMap();
        }
    }
}
