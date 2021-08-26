using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.WebApi.ViewModels.AuthViewModels
{
    public class AuthResponseViewModel
    {
        public string ErrorMessage { get; set; }
        public string Token { get; set; }

    }
}
