using System.Collections.Generic;

namespace VetClinic.WebApi.ViewModels.PetViewModels
{
    public class PetViewModelOrigin
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string ClientId { get; set; }
        public string AnimalType { get; set; }
        public int AnimalTypeId { get; set; }
        public ICollection<PetImageViewModel> PetImages { get; set; }
    }
}
