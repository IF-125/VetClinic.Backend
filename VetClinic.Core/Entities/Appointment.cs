using System;

namespace VetClinic.Core.Entities
{
    public enum AppointmentStatus
    {
        Opened = 1,
        Closed
    };

    public class Appointment
    {
        public int Id { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int? OrderProcedureId { get; set; }
        public OrderProcedure OrderProcedure { get; set; }
    }
}
