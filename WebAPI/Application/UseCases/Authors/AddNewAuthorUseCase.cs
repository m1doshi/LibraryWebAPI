using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Authors
{
    public class AddNewAuthorUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public AddNewAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> AddNewAuthor(AuthorModel author)
        {
            Author newAuthor = new Author
            {
                FirstName = author.FirstName,
                LastName = author.LastName,
                Birthday = author.Birthday,
                Country = author.Country
            };
            await unitOfWork.Authors.AddNewAuthor(newAuthor);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
