namespace VetClinic.WebApi.ViewModels.ScheduleViewModels
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Day { get; set; }
        public string EmployeeId { get; set; }
    }
}
