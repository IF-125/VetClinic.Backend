using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Entities
{
    public class AnimalType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public ICollection<Pet> Pets { get; set; }
        public ICollection<AnimalTypeProcedure> AnimalTypesProcedures { get; set; }
    }
}
