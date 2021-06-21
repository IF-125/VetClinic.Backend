using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Entities
{
    class AnimalTypes
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public ICollection<Pet> Pets { get; set; }
        
    }
}
