using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Book;
using WebAPI.Application.Interfaces.UnitOfWork;


namespace WebAPI.Application.UseCases.Books
{
    public class DeleteBookUseCase : IDeleteBookService
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteBookUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> DeleteBook(int bookId)
        {
            await unitOfWork.Books.DeleteBook(bookId);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
