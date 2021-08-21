using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels;
using VetClinic.WebApi.ViewModels.PetViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class PetMapperProfile : Profile
    {
        public PetMapperProfile()
        {
            #region PetViewModel mapping
            CreateMap<Pet, PetViewModel>()

                .ForMember(x => x.AnimalType, y => y.MapFrom(src => src.AnimalType.Type))

                .ReverseMap();
            #endregion

            #region PetResponseViewModel mapping
            CreateMap<Pet, PetResponseViewModel>()

                .ForMember(x => x.AnimalType, y => y.MapFrom(src => src.AnimalType.Type))

                .ForMember(x => x.Owner, y => y.MapFrom(src => src.Client.ToString()))

                .ReverseMap();
            #endregion
        }
    }
}
