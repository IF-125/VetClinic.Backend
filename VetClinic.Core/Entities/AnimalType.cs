using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Entities
{
    //TODO: replace with proper AnimalType entity
    public class AnimalType
    {
        public AnimalType()
        {
            this.Procedures = new HashSet<Procedure>();
        }
        public int Id { get; set; }
        public ICollection<Procedure> Procedures { get; set; }
    }
}
