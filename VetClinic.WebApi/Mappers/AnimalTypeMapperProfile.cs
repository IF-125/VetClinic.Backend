using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class AnimalTypeMapperProfile:Profile
    {
        public AnimalTypeMapperProfile()
        {
            CreateMap<AnimalType, AnimalTypeViewModel>().ReverseMap();
        }
    }
}
