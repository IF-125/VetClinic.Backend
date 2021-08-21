using FluentValidation;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(b => b.PaymentOption).NotEmpty().WithMessage("Payment option must be selected");
        }
    }
}
