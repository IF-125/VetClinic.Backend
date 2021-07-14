using FluentValidation;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class ProcedureValidator : AbstractValidator<Procedure>
    {
        public ProcedureValidator()
        {
            RuleFor(b => b.Title).NotEmpty().WithMessage("Procedure title is required!");
            RuleFor(b => b.Description).NotEmpty().WithMessage("Procedure description is required!");
            RuleFor(b => b.Title).Length(4, 50);
            RuleFor(b => b.Description).Length(4, 50);
        }
    }
}
