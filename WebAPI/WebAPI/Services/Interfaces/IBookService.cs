using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookModel>> GetAllBooks(int pageNumber, int pageSize);
        Task<BookModel> GetBookById(int bookId);
        Task<BookModel> GetBookByISBN(string isbn);
        Task<int> AddNewBook(BookModel book);
        Task<int> UpdateBook(int bookId, UpdateBookRequest data);
        Task<int> DeleteBook(int bookId);
        Task<int> UpdateImage(int bookId, IFormFile image);
        Task<bool> IssueBook(int bookId, int userId, DateTime returnDate);
        Task<bool> ReturnBook(int bookId);
    }
}
