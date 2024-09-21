using WebAPI.Application.Interfaces.Services.Authors;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Authors
{
    public class DeleteAuthorUseCase : IDeleteAuthorService
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> DeleteAuthor(int authorId)
        {
            await unitOfWork.Authors.DeleteAuthor(authorId);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
