using FluentValidation;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            Include(new UserValidator());

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Employee must have address");
        }
    }
}
