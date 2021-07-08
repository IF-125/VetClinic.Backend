using System.Collections.Generic;

namespace VetClinic.Core.Entities
{
    public class AnimalType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public ICollection<Pet> Pets { get; set; }
    }
}
