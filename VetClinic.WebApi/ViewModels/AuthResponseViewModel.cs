using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.WebApi.ViewModels
{
    public class AuthResponseViewModel
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
    }
}
