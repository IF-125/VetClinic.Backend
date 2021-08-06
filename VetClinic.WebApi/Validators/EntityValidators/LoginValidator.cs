using FluentValidation;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty()
               .WithMessage("Email is required");

            RuleFor(x => x.Password)
               .NotEmpty()
               .WithMessage("Password is required");
        }
    }
}
