using FluentValidation;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class SalaryValidator : AbstractValidator<Salary>
    {
        public SalaryValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Bonus).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
        }
    }
}
