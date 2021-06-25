using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Entities
{
    //TODO: replace with proper Pet entity
    public class Pet
    {
        public Pet()
        {
            this.OrderProcedures = new HashSet<OrderProcedure>();
        }
        public int Id { get; set; }
        public ICollection<OrderProcedure> OrderProcedures { get; set; }
    }
}
