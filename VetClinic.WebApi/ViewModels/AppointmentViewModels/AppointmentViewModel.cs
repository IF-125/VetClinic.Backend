namespace VetClinic.WebApi.ViewModels.AppointmentViewModels
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int OrderProcedureId { get; set; }
        public string DoctorName { get; set; }
    }
}
