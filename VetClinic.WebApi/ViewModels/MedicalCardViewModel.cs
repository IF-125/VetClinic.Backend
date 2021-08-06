namespace VetClinic.WebApi.ViewModels
{
    public class MedicalCardViewModel
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string TotalDuration { get; set; }
        public string Conclusion { get; set; }
        public string Details { get; set; }
        public string OrderDate { get; set; }
        public string ProcedureTitle { get; set; }
    }
}
