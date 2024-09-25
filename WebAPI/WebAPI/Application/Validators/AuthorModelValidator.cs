using FluentValidation;
using WebAPI.Application.DTOs;

namespace WebAPI.Application.Validators
{
    public class AuthorModelValidator : AbstractValidator<AuthorModel>
    {
        public AuthorModelValidator()
        {
            RuleFor(request => request.FirstName).NotEmpty().WithMessage("First name is requierd.")
                .Length(1, 20).WithMessage("Firstname length can't be more than 20 and less than 1.");
            RuleFor(request => request.LastName).NotEmpty().WithMessage("Last name is requierd.")
                .Length(1, 20).WithMessage("Lastname length can't be more than 20 and less than 1.");
            RuleFor(request => request.Country).MaximumLength(20)
                .WithMessage("Country length can't be more than 20.");
        }
    }
}
