using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Authors
{
    public class UpdateAuthorUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<bool> UpdateAuthor(int authorId, UpdateAuthorRequest data)
        {
            var result = await unitOfWork.Authors.UpdateAuthor(authorId, data);
            if (result == false)
                throw new EntityNotFoundException("Author", authorId);
            return result;
        }
    }
}
