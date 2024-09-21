using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Application.UseCases.Authors
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork unitOfWork;
        public AuthorService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<AuthorModel>> GetAllAuthors(int pageNumber, int pageSize)
        {
            return await unitOfWork.Authors.GetAllAuthors(pageNumber, pageSize);
        }
        public async Task<AuthorModel> GetAuthorById(int authorId)
        {
            return await unitOfWork.Authors.GetAuthorById(authorId);
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
            return await unitOfWork.Authors.GetAllBooksByAuthor(authorId);
        }
    }
}
