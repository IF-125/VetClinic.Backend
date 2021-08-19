using AutoMapper;
using System.Collections.Generic;
using VetClinic.Core.Entities;
using VetClinic.WebApi.ViewModels.OrderProcedureViewModels;
using VetClinic.WebApi.ViewModels.PetViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class PetMapperProfile : Profile
    {
        public PetMapperProfile()
        {
            #region PetViewModel mapping
            CreateMap<Pet, PetViewModel>()

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
