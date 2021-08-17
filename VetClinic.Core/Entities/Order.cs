using System;
using VetClinic.Core.Entities.Enums;

namespace VetClinic.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public PaymentOption PaymentOption { get; set; }
        public OrderProcedure OrderProcedure { get; set; }
    }
}
