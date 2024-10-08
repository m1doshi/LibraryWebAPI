using Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Books
{
    public class UpdateBookUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateBookUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> UpdateBook(int bookId, UpdateBookRequest data)
        {
            var book = await unitOfWork.Books.GetBookById(bookId);
            if (book == null) return 0;
            book.ISBN = data.ISBN;
            book.BookTitle = data.BookTitle;
            book.Genre = data.Genre;
            book.Description = data.Description;
            book.AuthorID = data.AuthorID;
            book.PickUpTime = data.PickUpTime;
            book.ReturnTime = data.ReturnTime;
            book.IsAvailable = data.IsAvailable;
            book.UserID = data.UserID;
            await unitOfWork.Books.UpdateBook(book);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
