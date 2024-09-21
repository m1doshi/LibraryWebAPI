using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services.Authors
{
    public interface IGetAllBooksByAuthorService
    {
        Task<IEnumerable<BookModel>> GetAllBooksByAuthor(int authorId);
    }
}
