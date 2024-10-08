using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;
using WebAPI.Core.Entities;

namespace WebAPI.Application.UseCases.Authors
{
    public class GetAllBooksByAuthorUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllBooksByAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<IEnumerable<BookModel>> GetAllBooksByAuthor(int authorId)
        {
            var books = await unitOfWork.Authors.GetAllBooksByAuthor(authorId);
            return books.Select(b => new BookModel
            {
                BookID = b.BookID,
                AuthorID = b.AuthorID,
                ISBN = b.ISBN,
                BookTitle = b.BookTitle,
                Genre = b.Genre,
                Description = b.Description
            });
        }
    }
}
