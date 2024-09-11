using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorModel>> GetAllAuthors(int pageNumber, int pageSize);
        Task<AuthorModel> GetAuthorById(int authorId);
        Task<int> AddNewAuthor(AuthorModel author);
        Task<int> UpdateAuthor(int authorId, UpdateAuthorRequest data);
        Task<int> DeleteAuthor(int authorId);
        Task<IEnumerable<BookModel>> GetAllBooksByAuthor(int authorId);
    }
}
