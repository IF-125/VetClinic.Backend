using FluentValidation;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class OrderProcedureValidator : AbstractValidator<OrderProcedure>
    {
        public OrderProcedureValidator()
        {
            RuleFor(b => b.Conclusion).NotEmpty().WithMessage("Conclusion is required!");
            RuleFor(b => b.PetId).NotEmpty().WithMessage("Pet id is required!");
            RuleFor(b => b.EmployeeId).NotEmpty().WithMessage("Employee id is required!");
            RuleFor(b => b.Procedure).NotEmpty().WithMessage("Procedure id is required!");
            RuleFor(b => b.Conclusion).Length(4, 50);
            RuleFor(b => b.Details).Length(4, 50);
        }
    }
}
