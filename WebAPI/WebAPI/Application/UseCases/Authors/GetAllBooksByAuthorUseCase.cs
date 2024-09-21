using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Authors;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Authors
{
    public class GetAllBooksByAuthorUseCase : IGetAllBooksByAuthorService
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllBooksByAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<BookModel>> GetAllBooksByAuthor(int authorId)
        {
            return await unitOfWork.Authors.GetAllBooksByAuthor(authorId);
        }
    }
}
