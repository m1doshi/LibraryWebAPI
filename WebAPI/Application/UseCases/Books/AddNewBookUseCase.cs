using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;
using WebAPI.Core.Entities;

namespace WebAPI.Application.UseCases.Books
{
    public class AddNewBookUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public AddNewBookUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> AddNewBook(BookModel model)
        {
            var book = new Book
            {
                AuthorID = model.AuthorID,
                BookTitle = model.BookTitle,
                ISBN = model.ISBN,
                Genre = model.Genre,
                Description = model.Description,
                Image = model.Image
            };
            await unitOfWork.Books.AddNewBook(book);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
