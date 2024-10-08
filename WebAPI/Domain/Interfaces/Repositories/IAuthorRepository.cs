using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;

namespace WebAPI.Core.Interfaces.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthors(int pageNumber, int pageSize);
        Task<Author> GetAuthorById(int authorId);
        Task<bool> AddNewAuthor(Author author);
        Task<bool> UpdateAuthor(Author author);
        Task<bool> DeleteAuthor(int authorId);
        Task<IEnumerable<Book>> GetAllBooksByAuthor(int authorId);
    }
}
