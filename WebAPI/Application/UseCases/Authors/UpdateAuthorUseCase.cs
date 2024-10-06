using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Authors
{
    public class UpdateAuthorUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> UpdateAuthor(int authorId, UpdateAuthorRequest data)
        {
            await unitOfWork.Authors.UpdateAuthor(authorId, data);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
