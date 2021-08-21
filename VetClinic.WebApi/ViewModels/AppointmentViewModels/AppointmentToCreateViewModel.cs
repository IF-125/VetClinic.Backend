using System;

namespace VetClinic.WebApi.ViewModels.AppointmentViewModels
{
    public class AppointmentToCreateViewModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Status { get; set; }
    }
}
