using System;
using System.Collections.Generic;

namespace VetClinic.Core.Entities
{
    public class EmployeePosition
    {
        public int Id { get; set; }
        public decimal CurrentBaseSalary { get; set; }
        public int Rate { get; set; }
        public DateTime? HierdDate { get; set; }
        public DateTime? DismissedDate { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public ICollection<Salary> Salaries { get; set; }
    }
}
