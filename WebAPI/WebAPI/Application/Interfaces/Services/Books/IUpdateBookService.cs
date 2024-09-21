using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services.Book
{
    public interface IUpdateBookService
    {
        Task<int> UpdateBook(int bookId, UpdateBookRequest data);
    }
}
