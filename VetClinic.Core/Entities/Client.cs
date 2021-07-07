using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.IdentityServer.Models;

namespace VetClinic.Core.Entities
{
    public class Client : User
    {
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
