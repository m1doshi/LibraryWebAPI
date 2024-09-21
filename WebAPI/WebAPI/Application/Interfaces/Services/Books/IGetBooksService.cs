using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services.Book
{
    public interface IGetBooksService
    {
        Task<IEnumerable<BookModel>> GetAllBooks(int pageNumber, int pageSize);
        Task<BookModel> GetBookById(int bookId);
        Task<BookModel> GetBookByISBN(string isbn);
    }
}
