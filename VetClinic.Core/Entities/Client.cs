using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.Core.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int UserId { get; set; }

        public IList<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
    }
}
