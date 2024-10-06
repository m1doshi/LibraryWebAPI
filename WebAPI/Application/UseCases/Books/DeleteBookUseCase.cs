using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Books
{
    public class DeleteBookUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteBookUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<int> DeleteBook(int bookId)
        {
            await unitOfWork.Books.DeleteBook(bookId);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
