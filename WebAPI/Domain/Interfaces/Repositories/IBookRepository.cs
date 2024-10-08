using Microsoft.AspNetCore.Http;
using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;

namespace WebAPI.Core.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks(int pageNumber, int pageSize);
        Task<Book> GetBookById(int bookId);
        Task<Book> GetBookByISBN(string isbn);
        Task<bool> AddNewBook(Book book);
        Task<bool> UpdateBook(Book book);
        Task<bool> DeleteBook(int bookId);
        Task<bool> UpdateImage(int bookId, byte[] imageData);
    }
}
