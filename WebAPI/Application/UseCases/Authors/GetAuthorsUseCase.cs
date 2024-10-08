using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Authors
{
    public class GetAuthorsUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAuthorsUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<IEnumerable<AuthorModel>> GetAllAuthors(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 1 : pageSize;
            var authors = await unitOfWork.Authors.GetAllAuthors(pageNumber, pageSize);
            return authors.Select(a => new AuthorModel
            {
                AuthorID = a.AuthorID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Birthday = a.Birthday,
                Country = a.Country
            });
        }
        public async virtual Task<AuthorModel> GetAuthorById(int authorId)
        {
            var entity = await unitOfWork.Authors.GetAuthorById(authorId);
            if (entity == null) return null;
            return new AuthorModel
            {
                AuthorID = entity.AuthorID,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Birthday = entity.Birthday,
                Country = entity.Country
            };
        }
    }
}
