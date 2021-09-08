using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinic.WebApi.ViewModels.EmployeeViewModels
{
    public class EmployeeToCreateViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
    }
}
