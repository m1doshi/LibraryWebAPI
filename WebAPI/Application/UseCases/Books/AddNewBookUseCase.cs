using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Books
{
    public class AddNewBookUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public AddNewBookUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> AddNewBook(BookModel model)
        {
            await unitOfWork.Books.AddNewBook(model);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
