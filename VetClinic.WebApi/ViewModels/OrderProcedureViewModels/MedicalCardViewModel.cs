namespace VetClinic.WebApi.ViewModels.OrderProcedureViewModels
{
    public class MedicalCardViewModel
    {
        public int Id { get; set; }
        public string TotalDuration { get; set; }
        public string Conclusion { get; set; }
        public string Details { get; set; }
        public string OrderDate { get; set; }
        public string ProcedureTitle { get; set; }
        public string PetName { get; set; }
        public string PetInformation { get; set; }
        public string PetBreed { get; set; }
        public int PetAge { get; set; }
        public string AnimalType { get; set; }
        public string Owner { get; set; }
    }
}
