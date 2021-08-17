using System.Collections.Generic;

namespace VetClinic.Core.Entities
{
    public class Client : User
    {
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public ICollection<Pet> Pets { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
