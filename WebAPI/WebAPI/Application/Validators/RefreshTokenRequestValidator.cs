using FluentValidation;
using WebAPI.Application.DTOs;

namespace WebAPI.Application.Validators
{
    public class RefreshTokenRequestValidator:AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(request => request.RefreshToken).NotEmpty().Length(32).WithMessage("Refresh token length must be equal to 32.");
        }
    }
}
