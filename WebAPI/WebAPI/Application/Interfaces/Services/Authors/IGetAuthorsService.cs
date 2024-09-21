using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Services.Authors
{
    public interface IGetAuthorsService
    {
        Task<IEnumerable<AuthorModel>> GetAllAuthors(int pageNumber, int pageSize);
        Task<AuthorModel> GetAuthorById(int authorId);
    }
}
