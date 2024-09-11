using WebAPI.Entities;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork unitOfWork;
        public BookService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<BookModel>> GetAllBooks(int pageNumber, int pageSize)
        {
            return await unitOfWork.Books.GetAllBooks(pageNumber, pageSize);
        }
        public async Task<BookModel> GetBookById(int bookId)
        {
            return await unitOfWork.Books.GetBookById(bookId);
        }
        public async Task<BookModel> GetBookByISBN(string isbn)
        {
            return await unitOfWork.Books.GetBookByISBN(isbn);
        }
        public async Task<int> AddNewBook(BookModel model)
        {
            await unitOfWork.Books.AddNewBook(model);
            return await unitOfWork.SaveChangesAsync();
        }
        public async Task<int> UpdateBook(int bookId, UpdateBookRequest data)
        {
            await unitOfWork.Books.UpdateBook(bookId, data);
            return await unitOfWork.SaveChangesAsync();
        }
        public async Task<int> DeleteBook(int bookId)
        {
            await unitOfWork.Books.DeleteBook(bookId);
            return await unitOfWork.SaveChangesAsync();
        }
        public async Task<int> UpdateImage(int bookId, IFormFile image)
        {
            await unitOfWork.Books.UpdateImage(bookId, image);
            return await unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IssueBook(int bookId, int userId, DateTime returnDate)
        {
            var book = await unitOfWork.Books.GetBookById(bookId);
            if (book == null || book.IsAvailable == 0)
            {
                throw new Exception("The book is unavailable");
            }
            var user = await unitOfWork.Users.GetUserById(userId);
            if (user == null) throw new Exception("User not found");

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
            if(book == null || book.IsAvailable == 1)
            {
                throw new Exception("The book is already returned or not found");
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
