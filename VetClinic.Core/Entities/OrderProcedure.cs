using System;

namespace VetClinic.Core.Entities
{
    public class OrderProcedure
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public TimeSpan Time { get; set; }
        public string Conclusion { get; set; }
        public string Details { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public int ProcedureId { get; set; }
        public Procedure Procedure { get; set; }
        public int PetId { get; set; }
        //public Pet Pet { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
