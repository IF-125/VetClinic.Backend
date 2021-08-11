using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels.ClientViewModels;

namespace VetClinic.WebApi.Mappers
{
    class ClientMapperProfile : Profile
    {
        public ClientMapperProfile()
        {
                 CreateMap<Client, ClientViewModel>().ReverseMap();
        }
    }
}
