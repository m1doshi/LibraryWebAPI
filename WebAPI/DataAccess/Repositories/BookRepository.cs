using WebAPI.Core.Interfaces.Repositories;
using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly MyDbContext dbContext;

        public BookRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Book>> GetAllBooks(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            return await dbContext.Books.Skip(skip).Take(pageSize).ToListAsync();
        }
        public async Task<Book> GetBookById(int bookId)
        {
            return await dbContext.Books.FindAsync(bookId);
        }
        public async Task<Book> GetBookByISBN(string isbn)
        {
            return await dbContext.Books.Where(b => b.ISBN == isbn).SingleOrDefaultAsync();
        }
        public async Task<bool> AddNewBook(Book book)
        {
            var result = await dbContext.Books.AddAsync(book);
            return result != null;
        }
        public async Task<bool> DeleteBook(int bookId)
        {
            var book = await dbContext.Books.FindAsync(bookId);
            if(book == null) return false;
            dbContext.Books.Remove(book);
            return true;
        }
        public async Task<bool> UpdateBook(Book book)
        {
            var result = dbContext.Books.Update(book);
            return result != null;
        }
        public async Task<bool> UpdateImage(int bookId, byte[] imageData)
        {
            var book = await dbContext.Books.FindAsync(bookId);
            if( book == null ) return false;
            book.Image = imageData;
            dbContext.Books.Update(book);
            return true;
        }
    }
}
