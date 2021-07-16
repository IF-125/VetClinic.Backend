using FluentValidation;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class ScheduleValidator : AbstractValidator<Schedule>
    {
        public ScheduleValidator()
        {
            RuleFor(x => x.Day)
                .NotEmpty();

            RuleFor(x => x.From)
                .NotEmpty()
                .WithMessage("Value for From property is required");

            RuleFor(x => x.To)
                .NotEmpty()
                .WithMessage("Value for To property is required");
        }
    }
}
