using System;

namespace VetClinic.WebApi.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
