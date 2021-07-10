using System.Collections.Generic;
using VetClinic.IdentityServer.Models;

namespace VetClinic.Core.Entities
{
    public class Client : User
    {
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public ICollection<Pet> Pets { get; set; }
    }
}
