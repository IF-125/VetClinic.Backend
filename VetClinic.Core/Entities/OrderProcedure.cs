namespace VetClinic.Core.Entities
{
    public enum OrderProcedureStatus
    {
        NotAssigned = 1,
        Assigned,
        Completed
    }
    public class OrderProcedure
    {
        public int Id { get; set; }
        public string Conclusion { get; set; }
        public string Details { get; set; }
        public OrderProcedureStatus Status { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Appointment Appointment { get; set; }
        public int? ProcedureId { get; set; }
        public Procedure Procedure { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
