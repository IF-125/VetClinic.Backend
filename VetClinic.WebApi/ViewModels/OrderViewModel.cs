using System;

namespace VetClinic.WebApi.ViewModels
{
    public class OrderViewModel
    {
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
