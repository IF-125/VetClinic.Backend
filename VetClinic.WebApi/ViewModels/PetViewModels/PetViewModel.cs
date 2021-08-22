using System.Collections.Generic;
using VetClinic.WebApi.ViewModels.PetViewModels;

namespace VetClinic.WebApi.ViewModels
{
    public class PetViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string ClientId { get; set; }
       
        public int AnimalTypeId { get; set; }
        public ICollection<PetImageViewModel> PetImages { get; set; }
    }
}
