using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Authors
{
    public class AddNewAuthorUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public AddNewAuthorUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> AddNewAuthor(AuthorModel author)
        {
            await unitOfWork.Authors.AddNewAuthor(author);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
