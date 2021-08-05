namespace VetClinic.WebApi.ViewModels
{
    public class OrderProcedureOfDoctorViewModel
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Time { get; set; }
        public string Conclusion { get; set; }
        public string Details { get; set; }
        public int OrderId { get; set; }
        public int? ProcedureId { get; set; }
        public int PetId { get; set; }
        public string EmployeeId { get; set; }
        public string ProcedureTitle { get;set; }
        public string PetName { get; set; }
        public string PetInformation { get; set; }
        public string PetBreed { get; set; }
        public int PetAge { get; set; }
        public string AnimalType { get; set; }
    }
}
