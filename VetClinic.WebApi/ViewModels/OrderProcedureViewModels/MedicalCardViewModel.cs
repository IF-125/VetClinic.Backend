namespace VetClinic.WebApi.ViewModels.OrderProcedureViewModels
{
    public class MedicalCardViewModel
    {
        public int Id { get; set; }
        public string TotalDuration { get; set; }
        public string Conclusion { get; set; }
        public string Status { get; set; }
        public string Details { get; set; }
        public string OrderDate { get; set; }
        public string ProcedureTitle { get; set; }
        public string EmployeeId { get; set; }
    }
}
