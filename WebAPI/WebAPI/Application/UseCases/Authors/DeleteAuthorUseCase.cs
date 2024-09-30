using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Authors
{
    public class DeleteAuthorUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> DeleteAuthor(int authorId)
        {
            await unitOfWork.Authors.DeleteAuthor(authorId);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
