using System;
using VetClinic.Core.Entities.Enums;

namespace VetClinic.WebApi.ViewModels.OrderViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public PaymentOption PaymentOption { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
