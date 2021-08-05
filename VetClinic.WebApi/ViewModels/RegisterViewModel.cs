using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VetClinic.WebApi.ViewModels
{
    public class RegisterViewModel
    {
        //[Required]
        //[Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        //[Display(Name = "Last Name")]
        public string LastName { get; set; }
        //[Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords doesn't match!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}
