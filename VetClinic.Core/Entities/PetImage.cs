using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Entities
{
    public class PetImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }
    }
}
