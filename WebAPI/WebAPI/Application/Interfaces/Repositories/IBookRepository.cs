using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookModel>> GetAllBooks(int pageNumber, int pageSize);
        Task<BookModel> GetBookById(int bookId);
        Task<BookModel> GetBookByISBN(string isbn);
        Task<bool> AddNewBook(BookModel book);
        Task<bool> UpdateBook(int bookId, UpdateBookRequest data);
        Task<bool> DeleteBook(int bookId);
        Task<bool> UpdateImage(int bookId, IFormFile image);
    }
}
