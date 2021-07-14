using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class OrderProcedureMapperProfile : Profile
    {
        public OrderProcedureMapperProfile()
        {
            CreateMap<OrderProcedure, OrderProcedureViewModel>().ReverseMap();
        }
    }
}
