using System.Collections.Generic;

namespace VetClinic.Core.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string ClientId { get; set; }
        public Client Client { get; set; }
        public int? AnimalTypeId { get; set; }
        public AnimalType AnimalType { get; set; }
        public ICollection<PetImage> PetImages { get; set; }
        public ICollection<OrderProcedure> OrderProcedures { get; set; }
    }
}
