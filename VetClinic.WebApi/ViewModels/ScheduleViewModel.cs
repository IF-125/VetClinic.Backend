using System;

namespace VetClinic.WebApi.ViewModels
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public string Day { get; set; }
    }
}
