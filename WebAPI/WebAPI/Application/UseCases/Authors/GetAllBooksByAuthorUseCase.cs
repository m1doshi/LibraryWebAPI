using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Authors
{
    public class GetAllBooksByAuthorUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllBooksByAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<IEnumerable<BookModel>> GetAllBooksByAuthor(int authorId)
        {
            var result = await unitOfWork.Authors.GetAllBooksByAuthor(authorId);
            return result;
        }
    }
}
