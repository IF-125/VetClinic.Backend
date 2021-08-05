using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class RegisterValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName.)
               .NotEmpty();

        }

    }
}
