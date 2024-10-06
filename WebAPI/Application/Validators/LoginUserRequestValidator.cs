using FluentValidation;
using WebAPI.Core.DTOs;

namespace WebAPI.Application.Validators
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(request => request.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(request => request.Email).NotEmpty().WithMessage("Email is required");
        }
    }
}
