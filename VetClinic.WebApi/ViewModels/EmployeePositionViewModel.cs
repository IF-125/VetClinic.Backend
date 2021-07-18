using System;

namespace VetClinic.WebApi.ViewModels
{
    public class EmployeePositionViewModel
    {
        public int Id { get; set; }
        public decimal CurrentBaseSalary { get; set; }
        public int Rate { get; set; }
        public DateTime? HierdDate { get; set; }
        public DateTime? DismissedDate { get; set; }
        public string EmployeeId { get; set; }
        public int PositionId { get; set; }
        public string EmployeeFullName { get; set; }
        public string Address { get; set; }
        public string PositionName { get; set; }
    }
}
