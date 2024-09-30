using FluentValidation;
using WebAPI.Application.DTOs;

namespace WebAPI.Application.Validators
{
    public class RegisterUserRequestValidator:AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(request => request.UserName).NotEmpty().WithMessage("Username is required")
                .Length(3, 20).WithMessage("Username length can't be more than 20 and less than 3.");
            RuleFor(request => request.Password).NotEmpty().WithMessage("Password is required")
                .Length(7, 30).WithMessage("Password length can't be more than 30 and less than 7.");
            RuleFor(request => request.Email).NotEmpty().WithMessage("Email is required")
                .Length(6, 40).WithMessage("Email length can't be more than 40 and less than 6.");
        }
    }
}
