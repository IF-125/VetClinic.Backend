namespace VetClinic.WebApi.ViewModels
{
    public class PetViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public int ClientId { get; set; }
        public int AnimalTypeId { get; set; }
    }
}
