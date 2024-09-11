using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository authorRepository;
        private readonly IUnitOfWork unitOfWork;
        public AuthorService (IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            this.authorRepository = authorRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<AuthorModel>> GetAllAuthors(int pageNumber, int pageSize)
        {
            return await authorRepository.GetAllAuthors(pageNumber,pageSize);
        }
        public async Task<AuthorModel> GetAuthorById(int authorId)
        {
            return await authorRepository.GetAuthorById(authorId);
        }
        public async Task<int> AddNewAuthor(AuthorModel author)
        {
            await unitOfWork.Authors.AddNewAuthor(author);
            return await unitOfWork.SaveChangesAsync();
        }
        public async Task<int> UpdateAuthor(int authorId, UpdateAuthorRequest data)
        {
            await unitOfWork.Authors.UpdateAuthor(authorId, data);
            return await unitOfWork.SaveChangesAsync();
        }
        public async Task<int> DeleteAuthor(int authorId)
        {
            await unitOfWork.Authors.DeleteAuthor(authorId);
            return await unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<BookModel>> GetAllBooksByAuthor(int authorId)
        {
            return await authorRepository.GetAllBooksByAuthor(authorId);
        }
    }
}
