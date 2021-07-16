using System;

namespace VetClinic.WebApi.ViewModels
{
    public class OrderProcedureViewModel
    {
        public int Count { get; set; }
        public string Time { get; set; }
        public string Conclusion { get; set; }
        public string Details { get; set; }
        public int OrderId { get; set; }
        public int ProcedureId { get; set; }
        public int PetId { get; set; }
        public string EmployeeId { get; set; }
    }
}
