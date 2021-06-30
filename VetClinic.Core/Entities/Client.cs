using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.IdentityServer.Models;

namespace VetClinic.Core.Entities
{
    public class Client : User
    {
        public IList<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
    }
}
