using WebAPI.Core.DTOs;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Books
{
    public class GetBooksUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public GetBooksUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async virtual Task<IEnumerable<BookModel>> GetAllBooks(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 1 : pageSize;
            return await unitOfWork.Books.GetAllBooks(pageNumber, pageSize);
        }
        public async virtual Task<BookModel> GetBookById(int bookId)
        {
            var result = await unitOfWork.Books.GetBookById(bookId);
            return result;
        }
        public async virtual Task<BookModel> GetBookByISBN(string isbn)
        {
            var result = await unitOfWork.Books.GetBookByISBN(isbn);
            return result;
        }
    }
}
