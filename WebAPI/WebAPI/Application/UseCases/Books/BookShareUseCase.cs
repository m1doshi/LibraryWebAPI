using WebAPI.Application.DTOs;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Books
{
    public class BookShareUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public BookShareUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<bool> IssueBook(IssueBookRequest request)
        {
            var book = await unitOfWork.Books.GetBookById(request.BookId);
            var bookModel = new UpdateBookRequest();
            bookModel.PickUpTime = DateTime.Now;
            bookModel.ReturnTime = request.ReturnDate;
            bookModel.UserID = request.UserId;
            bookModel.IsAvailable = 0;

            await unitOfWork.Books.UpdateBook(book.BookID, bookModel);

            return await unitOfWork.SaveChangesAsync() > 0;
        }

        public async virtual Task<bool> ReturnBook(int bookId)
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
