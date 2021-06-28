using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Entities
{
    public class PhoneNumber
    {
        public int Id { get; set; }

        public string Phone { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}
