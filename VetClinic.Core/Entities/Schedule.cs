using System;

namespace VetClinic.Core.Entities
{
    public enum Days
    {
        Monday = 1,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

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
