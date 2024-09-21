using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Book;
using WebAPI.Application.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Books
{
    public class UpdateBookUseCase : IUpdateBookService
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateBookUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> UpdateBook(int bookId, UpdateBookRequest data)
        {
            await unitOfWork.Books.UpdateBook(bookId, data);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
