using WebAPI.Application.DTOs;

namespace WebAPI.Application.Interfaces.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<AuthorModel>> GetAllAuthors(int pageNumber, int pageSize);
        Task<AuthorModel> GetAuthorById(int authorId);
        Task<bool> AddNewAuthor(AuthorModel author);
        Task<bool> UpdateAuthor(int authorId, UpdateAuthorRequest data);
        Task<bool> DeleteAuthor(int authorId);
        Task<IEnumerable<BookModel>> GetAllBooksByAuthor(int authorId);
    }
}
