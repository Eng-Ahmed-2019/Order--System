using FluentValidation;
using OrderSystem.Application.DTOs;

namespace OrderSystem.Application.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequestDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty()
                .WithMessage("Order number is required")
                .MaximumLength(50)
                .WithMessage("Length of order number must be =50 at least");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0)
                .WithMessage("Total amount must be greater than 0");
        }
    }
}