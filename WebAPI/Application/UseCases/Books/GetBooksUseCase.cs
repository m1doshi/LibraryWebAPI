using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;
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
            var books = await unitOfWork.Books.GetAllBooks(pageNumber, pageSize);
            return books.Select(b => new BookModel
            {
                BookID = b.BookID,
                ISBN = b.ISBN,
                BookTitle = b.BookTitle,
                Genre = b.Genre,
                Description = b.Description,
                AuthorID = b.AuthorID,
                PickUpTime = b.PickUpTime,
                ReturnTime = b.ReturnTime
            });
        }
        public async virtual Task<BookModel> GetBookById(int bookId)
        {
            var entity = await unitOfWork.Books.GetBookById(bookId);
            if (entity == null) return null;
            return new BookModel
            {
                BookID = entity.BookID,
                ISBN = entity.ISBN,
                BookTitle = entity.BookTitle,
                Genre = entity.Genre,
                Description = entity.Description,
                AuthorID = entity.AuthorID,
                PickUpTime = entity.PickUpTime,
                ReturnTime = entity.ReturnTime
            };
        }
        public async virtual Task<BookModel> GetBookByISBN(string isbn)
        {
            var entity = await unitOfWork.Books.GetBookByISBN(isbn);
            if (entity == null) return null;
            return new BookModel
            {
                BookID = entity.BookID,
                ISBN = entity.ISBN,
                BookTitle = entity.BookTitle,
                Genre = entity.Genre,
                Description = entity.Description,
                AuthorID = entity.AuthorID,
                PickUpTime = entity.PickUpTime,
                ReturnTime = entity.ReturnTime
            };
        }
    }
}
