using System;

namespace VetClinic.WebApi.ViewModels
{
    public class SalaryViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Bonus { get; set; }
        public int EmployeePositionId { get; set; }
    }
}
