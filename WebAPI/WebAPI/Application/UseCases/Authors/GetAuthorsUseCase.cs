using System.Runtime.CompilerServices;
using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Authors
{
    public class GetAuthorsUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAuthorsUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<AuthorModel>> GetAllAuthors(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 1 : pageSize;
            return await unitOfWork.Authors.GetAllAuthors(pageNumber, pageSize);
        }
        public async Task<AuthorModel> GetAuthorById(int authorId)
        {
            var result = await unitOfWork.Authors.GetAuthorById(authorId);
            if (result == null)
                throw new EntityNotFoundException("Author", authorId);
            return result;
        }
    }
}
