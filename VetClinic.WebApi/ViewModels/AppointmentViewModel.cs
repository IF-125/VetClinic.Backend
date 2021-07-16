using System;

using VetClinic.Core.Entities;

namespace VetClinic.WebApi.ViewModels
{
    public class AppointmentViewModel
    {
        public AppointmentStatus Status { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
