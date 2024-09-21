using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services.Book
{
    public interface IAddNewBookService
    {
        Task<int> AddNewBook(BookModel book);
    }
}
