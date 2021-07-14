using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

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
