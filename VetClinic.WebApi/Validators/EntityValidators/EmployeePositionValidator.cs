using FluentValidation;
using VetClinic.Core.Entities;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class EmployeePositionValidator : AbstractValidator<EmployeePosition>
    {
        public EmployeePositionValidator()
        {
            RuleFor(x => x.CurrentBaseSalary)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.Rate)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.EmployeeId)
                .NotEmpty()
                .WithMessage($"Employee {IdIsRequired}");

            RuleFor(x => x.PositionId)
                .NotEmpty()
                .WithMessage($"Position {IdIsRequired}");
        }
    }
}
