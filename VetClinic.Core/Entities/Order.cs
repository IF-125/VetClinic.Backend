using System;

namespace VetClinic.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPaid { get; set; }
        public int OrderProcedureId { get; set; }
        public OrderProcedure OrderProcedure { get; set; }
    }
}
