using FluentValidation;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class ScheduleValidator : AbstractValidator<Schedule>
    {
        public ScheduleValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty()
                .WithMessage("Employee id is required");

            RuleFor(x => x.Day)
                .NotNull();

            RuleFor(x => x.From)
                .NotEmpty()
                .WithMessage("Value for From property is required");

            RuleFor(x => x.To)
                .NotEmpty()
                .WithMessage("Value for To property is required");
        }
    }
}
