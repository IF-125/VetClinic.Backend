using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public virtual OrderProcedure OrderProcedure { get; set; }
        public string Status { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
    }
}
