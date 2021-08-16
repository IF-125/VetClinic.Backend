using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.WebApi.ViewModels
{
    public class PetImageViewModel
    {
        public int? Id { get; set; }
        public string Path { get; set; }
        public int PetId { get; set; }
    }
}
