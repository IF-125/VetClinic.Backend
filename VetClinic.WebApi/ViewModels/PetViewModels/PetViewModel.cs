namespace VetClinic.WebApi.ViewModels.PetViewModels
{
    public class PetViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string ClientId { get; set; }
        public int AnimalTypeId { get; set; }
    }
}
