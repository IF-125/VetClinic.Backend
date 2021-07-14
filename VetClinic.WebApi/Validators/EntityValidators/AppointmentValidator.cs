using FluentValidation;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class AppointmentValidator : AbstractValidator<Appointment>
    {
        public AppointmentValidator()
        {
            RuleFor(b => b.Status).NotEmpty().WithMessage("Appointment status is required!");
            RuleFor(b => b.From).NotEmpty().WithMessage("Appointment starting date is required!");
            RuleFor(b => b.To).NotEmpty().WithMessage("Appointment ending date is required!");
        }
    }
}
