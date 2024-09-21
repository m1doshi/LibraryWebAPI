using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Authors;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Infrastructures.Persistence;

namespace WebAPI.Application.UseCases.Authors
{
    public class AddNewAuthorUseCase : IAddNewAuthorService
    {
        private readonly IUnitOfWork unitOfWork;
        public AddNewAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> AddNewAuthor(AuthorModel author)
        {
            await unitOfWork.Authors.AddNewAuthor(author);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
