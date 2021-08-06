using FluentValidation;
using VetClinic.WebApi.ViewModels;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class RegisterValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName)
               .NotEmpty()
               .WithMessage("Value is required");
               
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Value is required"); 

            RuleFor(x => x.Email)
               .NotEmpty()
               .WithMessage("Email is required");

            RuleFor(x => x.Password)
               .NotEmpty()
               .WithMessage("Password is required");

            RuleFor(x => x.PasswordConfirm)
                .NotEmpty()
                .Equal(x => x.Password)
                .WithMessage("Passwords doesn't match!");
        }
    }
}
