﻿namespace VetClinic.WebApi.ViewModels.PetViewModels
{
    public class PetResponseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string ClientId { get; set; }
        public string AnimalType { get; set; }
    }
}
