using System;
using System.Collections.Generic;

namespace VetClinic.Core.Entities
{
    public class Procedure
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Duration { get; set; }
        public decimal Price { get; set; }
        public ICollection<OrderProcedure> OrderProcedures { get; set; }
        public ICollection<AnimalType> AnimalTypes { get; set; }
    }
}
