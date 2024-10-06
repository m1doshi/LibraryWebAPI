using FluentValidation;
using WebAPI.Core.DTOs;

namespace WebAPI.Application.Validators
{
    public class RoleModelValidator : AbstractValidator<RoleModel>
    {
        public RoleModelValidator()
        {
            RuleFor(model => model.RoleName).NotEmpty().WithMessage("Role name is requierd.");
        }
    }
}
