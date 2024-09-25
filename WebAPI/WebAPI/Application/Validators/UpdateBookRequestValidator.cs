using FluentValidation;
using WebAPI.Application.DTOs;

namespace WebAPI.Application.Validators
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(request => request.ISBN).Matches(@"^\d{3}-\d{1}-\d{4}-\d{4}-\d{1}$")
                .WithMessage("ISBN must be in format: xxx-x-xxxx-xxxx-x, x - number");
            RuleFor(request => request.BookTitle).Length(2, 100)
                .WithMessage("Title length can't be more than 100 and less than 2.");
            RuleFor(request => request.Genre).Length(3, 30)
                .WithMessage("Genre length can't be more than 30 and less than 3.");
            RuleFor(request => request.Description).Length(0,300)
                .WithMessage("Description length can't be more than 300");
        }
    }
}
