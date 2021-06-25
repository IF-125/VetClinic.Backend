using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Entities
{
    public class OrderProcedure
    {
        public int Id { get; set; }
        public virtual Order Order { get; set; }
        public virtual Appointment Appointment { get; set; }
        public Procedure Procedure { get; set; }
        public Pet Pet { get; set; }
        public Employee Employee { get; set; }
        public int Count { get; set; }
        public DateTime Time { get; set; }
        public string Conclusion { get; set; }
        public string Details { get; set; }
    }
}
