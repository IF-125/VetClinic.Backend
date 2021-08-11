using System;
using VetClinic.Core.Entities.Enums;

namespace VetClinic.Core.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public Days? Day { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
