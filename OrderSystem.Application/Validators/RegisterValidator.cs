using FluentValidation;
using OrderSystem.Application.DTOs;

namespace OrderSystem.Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required")
                .MaximumLength(50).WithMessage("FullName must be less than 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email format is not valid");

            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("NationalId is required")
                .Must(NationalIdValidator.IsValid)
                .WithMessage("National ID is not valid");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Custom((password, context) =>
                {
                    var error = PasswordValidator.Validate(password);
                    if (error != null)
                    {
                        context.AddFailure(error);
                    }
                });
        }
    }
}