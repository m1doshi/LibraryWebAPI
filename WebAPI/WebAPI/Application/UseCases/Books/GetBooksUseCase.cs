using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Book;
using WebAPI.Application.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Books
{
    public class GetBooksUseCase : IGetBooksService
    {
        private readonly IUnitOfWork unitOfWork;
        public GetBooksUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<BookModel>> GetAllBooks(int pageNumber, int pageSize)
        {
            return await unitOfWork.Books.GetAllBooks(pageNumber, pageSize);
        }
        public async Task<BookModel> GetBookById(int bookId)
        {
            return await unitOfWork.Books.GetBookById(bookId);
        }
        public async Task<BookModel> GetBookByISBN(string isbn)
        {
            return await unitOfWork.Books.GetBookByISBN(isbn);
        }
    }
}
