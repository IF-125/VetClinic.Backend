using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Entities
{
    //TODO: replace with proper Employee entity
    public class Employee
    {
        public Employee(){
            this.OrderProcedures = new HashSet<OrderProcedure>();
        }
        public int Id { get; set; }
        public ICollection<OrderProcedure> OrderProcedures { get; set; }
    }
}
