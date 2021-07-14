using FluentValidation;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(b => b.CreatedAt).NotEmpty().WithMessage("Order creation date is required!");
        }
    }
}
