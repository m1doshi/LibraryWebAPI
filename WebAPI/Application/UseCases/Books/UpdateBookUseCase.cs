using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Books
{
    public class UpdateBookUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateBookUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> UpdateBook(int bookId, UpdateBookRequest data)
        {
            await unitOfWork.Books.UpdateBook(bookId, data);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
