using FluentValidation;
using VetClinic.Core.Entities;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class EmployeePositionValidator : AbstractValidator<EmployeePosition>
    {
        public EmployeePositionValidator()
        {
            RuleFor(x => x.CurrentBaseSalary).NotEmpty();
            RuleFor(x => x.Rate).NotEmpty();
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage($"Employee {IdIsRequired}");
            RuleFor(x => x.PositionId).NotEmpty().WithMessage($"Position {IdIsRequired}");
        }
    }
}
