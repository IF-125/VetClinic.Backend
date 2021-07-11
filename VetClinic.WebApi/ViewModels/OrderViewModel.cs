using System;

namespace VetClinic.WebApi.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public bool IsPaid { get; set; }
        public int OrderProcedureId { get; set; }
    }
}
