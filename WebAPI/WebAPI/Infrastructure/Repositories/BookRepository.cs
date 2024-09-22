using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructures.Persistence;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using System.Net;
using System.Reflection.Metadata;

namespace WebAPI.Infrastructures.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly MyDbContext dbContext;

        public BookRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<BookModel>> GetAllBooks(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            return await dbContext.Books.Skip(skip).Take(pageSize).Select(b => new BookModel
            {
                BookID = b.BookID,
                ISBN = b.ISBN,
                BookTitle = b.BookTitle,
                Genre = b.Genre,
                Description = b.Description,
                AuthorID = b.AuthorID,
                PickUpTime = b.PickUpTime,
                ReturnTime = b.ReturnTime,
            }).ToListAsync();
        }
        public async Task<BookModel> GetBookById(int bookId)
        {
            var book = await dbContext.Books.FindAsync(bookId);
            if (book == null)
                throw new EntityNotFoundException("Book", bookId);
            return new BookModel
            {
                BookID = book.BookID,
                ISBN = book.ISBN,
                BookTitle = book.BookTitle,
                Genre = book.Genre,
                Description = book.Description,
                AuthorID = book.AuthorID,
                PickUpTime = book.PickUpTime,
                ReturnTime = book.ReturnTime,
            };
        }
        public async Task<BookModel> GetBookByISBN(string isbn)
        {
            var book = dbContext.Books.Where(b=>b.ISBN == isbn).FirstOrDefault();
            if (book == null)
                throw new EntityNotFoundException("Book", isbn);
            return new BookModel
            {
                BookID = book.BookID,
                ISBN = book.ISBN,
                BookTitle = book.BookTitle,
                Genre = book.Genre,
                Description = book.Description,
                AuthorID = book.AuthorID,
                PickUpTime = book.PickUpTime,
                ReturnTime = book.ReturnTime,
            };
        }
        public async Task<bool> AddNewBook(BookModel book)
        {
            Book newBook = new();
            newBook.ISBN = book.ISBN;
            newBook.BookTitle = book.BookTitle;
            newBook.Genre = book.Genre;
            newBook.Description = book.Description;
            newBook.AuthorID = book.AuthorID;
            newBook.PickUpTime = book.PickUpTime;
            newBook.ReturnTime = book.ReturnTime;
            return await dbContext.Books.AddAsync(newBook) != null;
        }

        public async Task<bool> DeleteBook(int bookId)
        {
            var book = await dbContext.Books.FindAsync(bookId);
            if (book == null)
                throw new EntityNotFoundException("Book", bookId);
            dbContext.Remove(book);
            return true;
        }

        public async Task<bool> UpdateBook(int bookId, UpdateBookRequest data)
        {
            var book = await dbContext.Books.FindAsync(bookId);
            if (book == null)
                throw new EntityNotFoundException("Book", bookId);
            book.ISBN = data.ISBN;
            book.BookTitle = data.BookTitle;
            book.Genre = data.Genre;
            book.Description = data.Description;
            book.AuthorID = data.AuthorID;
            book.PickUpTime = data.PickUpTime;
            book.ReturnTime = data.ReturnTime;
            book.IsAvailable = data.IsAvailable;
            book.UserID = data.UserID;
            return true;
        }
        public async Task<bool> UpdateImage(int bookId, IFormFile image)
        {
            var book = await dbContext.Books.FindAsync(bookId);
            if (book == null)
                throw new EntityNotFoundException("Book", bookId);
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                book.Image = memoryStream.ToArray();
            }
            return true;
        }
    }
}
