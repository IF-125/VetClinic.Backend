using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class PetMapperProfile:Profile
    {
        public PetMapperProfile()
        {
            CreateMap<Pet, PetViewModel>().ReverseMap();
        }
    }
}
