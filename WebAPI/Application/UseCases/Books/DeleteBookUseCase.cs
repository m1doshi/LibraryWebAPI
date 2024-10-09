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
            var book = await unitOfWork.Books.GetBookById(bookId);
            if (book == null) return 0;
            await unitOfWork.Books.DeleteBook(bookId);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
