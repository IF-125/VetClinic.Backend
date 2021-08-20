using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class PetImageMapperProfile:Profile
    {
        public PetImageMapperProfile()
        {
            CreateMap<PetImage, PetImageViewModel>()
                .ReverseMap();
        }
    }
}
