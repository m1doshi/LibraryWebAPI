using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;


namespace WebAPI.Application.UseCases.Books
{
    public class DeleteBookUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteBookUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> DeleteBook(int bookId)
        {
            var result = await unitOfWork.Books.DeleteBook(bookId);
            if (result == false)
                throw new EntityNotFoundException("Book", bookId);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
