using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Authors;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Authors
{
    public class UpdateAuthorUseCase : IUpdateAuthorService
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> UpdateAuthor(int authorId, UpdateAuthorRequest data)
        {
            await unitOfWork.Authors.UpdateAuthor(authorId, data);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
