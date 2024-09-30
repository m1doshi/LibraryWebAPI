using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;

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
