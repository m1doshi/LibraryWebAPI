using FluentValidation;
using WebAPI.Application.DTOs;

namespace WebAPI.Application.Validators
{
    public class UserModelValidator:AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(user => user.UserName).NotEmpty().WithMessage("Username is required.")
                .Length(3, 20).WithMessage("Username length can't be more than 20 and less than 3.");
            RuleFor(user => user.Email).NotEmpty().WithMessage("Email is required")
                .Length(6, 40).WithMessage("Email length can't be more than 40 and less than 6.");
            RuleFor(user => user.RoleID).NotEmpty().LessThan(4).WithMessage("RoleID = 1 - Admin, 2 - User, 3 - Librarian");
        }
    }
}
