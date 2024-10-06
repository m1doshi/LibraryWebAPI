using WebAPI.Core.DTOs;
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
            return await unitOfWork.Authors.GetAllAuthors(pageNumber, pageSize);
        }
        public async virtual Task<AuthorModel> GetAuthorById(int authorId)
        {
            var result = await unitOfWork.Authors.GetAuthorById(authorId);
            return result;
        }
    }
}
