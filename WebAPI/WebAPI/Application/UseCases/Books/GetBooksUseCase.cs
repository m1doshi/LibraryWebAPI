using System.Net;
using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;

namespace WebAPI.Application.UseCases.Books
{
    public class GetBooksUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public GetBooksUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<BookModel>> GetAllBooks(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 1 : pageSize;
            return await unitOfWork.Books.GetAllBooks(pageNumber, pageSize);
        }
        public async Task<BookModel> GetBookById(int bookId)
        {
            var result = await unitOfWork.Books.GetBookById(bookId);
            if (result == null)
                throw new EntityNotFoundException("Book", bookId);
            return result;
        }
        public async Task<BookModel> GetBookByISBN(string isbn)
        {
            var result = await unitOfWork.Books.GetBookByISBN(isbn);
            if (result == null)
                throw new EntityNotFoundException("Book", isbn);
            return result;
        }
    }
}
