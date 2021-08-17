using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels.AnimalTypesViewModels;

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
