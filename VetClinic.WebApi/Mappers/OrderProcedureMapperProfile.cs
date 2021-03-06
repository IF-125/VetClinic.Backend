using AutoMapper;
using VetClinic.Core.Entities;
using VetClinic.WebApi.Converters;
using VetClinic.WebApi.ViewModels.OrderProcedureViewModels;

namespace VetClinic.WebApi.Mappers
{
    public class OrderProcedureMapperProfile : Profile
    {
        public OrderProcedureMapperProfile()
        {
            #region OrderProcedureViewModel mapping
            CreateMap<OrderProcedure, OrderProcedureViewModel>()
                
                .ReverseMap();
            #endregion

            #region OrderProcedureOfDoctorViewModel mapping
            CreateMap<OrderProcedure, OrderProcedureOfDoctorViewModel>()

                .ForMember(x => x.ProcedureTitle, y => y.MapFrom(src => src.Procedure.Title))

                .ForMember(x => x.PetName, y => y.MapFrom(src => src.Pet.Name))

                .ForMember(x => x.PetInformation, y => y.MapFrom(src => src.Pet.Information))

                .ForMember(x => x.PetBreed, y => y.MapFrom(src => src.Pet.Breed))

                .ForMember(x => x.PetAge, y => y.MapFrom(src => src.Pet.Age))

                .ForMember(x => x.Duration, opt => opt.ConvertUsing(new StringToTimeSpanConverter(), src => src.Procedure.Duration))

                .ForMember(x => x.AnimalType, y => y.MapFrom(src => src.Pet.AnimalType.Type))

                .ReverseMap();
            #endregion

            #region MedicalCardViewModel mapping
            CreateMap<OrderProcedure, MedicalCardViewModel>()

                .ForMember(x => x.TotalDuration, y => y.MapFrom(src => src.Procedure.Duration))

                .ForMember(x => x.OrderDate, y => y.MapFrom(src => src.Order.CreatedAt))

                .ForMember(x => x.ProcedureTitle, y => y.MapFrom(src => src.Procedure.Title))

                .ForMember(x => x.Status, y => y.MapFrom(src => src.Status.ToString()))

                .ReverseMap();
            #endregion
        }
    }
}
