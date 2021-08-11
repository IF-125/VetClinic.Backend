using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels.OrderViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class OrderMapperProfile : Profile
    {
        public OrderMapperProfile()
        {
            CreateMap<Order, OrderViewModel>().ReverseMap();
        }
    }
}
