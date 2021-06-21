using System;

namespace VetClinic.Core.Entities
{
    public class Salary
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Bonus { get; set; }
        public int EmployeePositionId { get; set; }
        public EmployeePosition EmployeePosition { get; set; }

    }
}
