using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.Converters;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class OrderProcedureMapperProfile : Profile
    {
        public OrderProcedureMapperProfile()
        {
            CreateMap<OrderProcedure, OrderProcedureViewModel>()
                
                .ReverseMap();


            CreateMap<OrderProcedure, OrderProcedureOfDoctorViewModel>()

                .ForMember(x => x.ProcedureTitle, y => y.MapFrom(src => src.Procedure.Title))

                .ForMember(x => x.PetName, y => y.MapFrom(src => src.Pet.Name))

                .ForMember(x => x.PetInformation, y => y.MapFrom(src => src.Pet.Information))

                .ForMember(x => x.PetBreed, y => y.MapFrom(src => src.Pet.Breed))

                .ForMember(x => x.PetAge, y => y.MapFrom(src => src.Pet.Age))

                .ForMember(x => x.Time, opt => opt.ConvertUsing(new StringToTimeSpanConverter()))

                .ReverseMap();
        }
    }
}
