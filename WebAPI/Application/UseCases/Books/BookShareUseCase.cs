using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces.UnitOfWork;
using WebAPI.DataAccess.Exceptions;

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
            if (book == null)
            {
                throw new EntityNotFoundException("Book", request.BookId);
            }
            if(book.IsAvailable == 0)
            {
                throw new BusinessRuleViolationException("The book is not available now");
            }
            book.PickUpTime = DateTime.Now;
            book.ReturnTime = request.ReturnDate;
            book.UserID = request.UserId;
            book.IsAvailable = 0;
            await unitOfWork.Books.UpdateBook(book);
            return await unitOfWork.SaveChangesAsync() > 0;
        }

        public async virtual Task<bool> ReturnBook(int bookId)
        {
            var book = await unitOfWork.Books.GetBookById(bookId);
            if (book == null)
            {
                throw new EntityNotFoundException("Book", bookId);
            }
            if (book.IsAvailable == 1)
            {
                throw new BusinessRuleViolationException("The book is already returned");
            }
            book.PickUpTime = null;
            book.ReturnTime = null;
            book.UserID = null;
            book.IsAvailable = 1;
            await unitOfWork.Books.UpdateBook(book);
            return await unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
