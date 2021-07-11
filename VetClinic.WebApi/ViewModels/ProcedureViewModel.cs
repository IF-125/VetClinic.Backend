using System;

namespace VetClinic.WebApi.ViewModels
{
    public class ProcedureViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
    }
}
