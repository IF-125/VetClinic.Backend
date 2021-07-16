using System.Collections.Generic;

namespace VetClinic.WebApi.ViewModels
{
    public class ClientViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<PhoneNumberViewModel> PhoneNumbers { get; set; }

    }

}
