using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Book;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Domain.Exceptions;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Books
{
    public class BookShareUseCase : IBookShareService
    {
        private readonly IUnitOfWork unitOfWork;
        public BookShareUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> IssueBook(int bookId, int userId, DateTime returnDate)
        {
            var book = await unitOfWork.Books.GetBookById(bookId);
            if (book == null || book.IsAvailable == 0)
            {
                throw new BusinessRuleViolationException("The book is unavailable");
            }
            var user = await unitOfWork.Users.GetUserById(userId);
            var bookModel = new UpdateBookRequest();
            bookModel.PickUpTime = DateTime.Now;
            bookModel.ReturnTime = returnDate;
            bookModel.UserID = userId;
            bookModel.IsAvailable = 0;

            await unitOfWork.Books.UpdateBook(book.BookID, bookModel);

            return await unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> ReturnBook(int bookId)
        {
            var book = await unitOfWork.Books.GetBookById(bookId);
            if (book == null || book.IsAvailable == 1)
            {
                throw new BusinessRuleViolationException("The book is already returned or not found");
            }
            var bookModel = new UpdateBookRequest();
            bookModel.PickUpTime = null;
            bookModel.ReturnTime = null;
            bookModel.UserID = null;
            bookModel.IsAvailable = 1;
            await unitOfWork.Books.UpdateBook(book.BookID, bookModel);
            return await unitOfWork.SaveChangesAsync() > 0;

        }
    }
}
