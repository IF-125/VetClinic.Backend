using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    class PetMapperProfile:Profile
    {
        public PetMapperProfile()
        {
            CreateMap<Pet, PetViewModel>().ReverseMap();
        }
    }
}
