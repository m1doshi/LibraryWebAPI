using Microsoft.EntityFrameworkCore;
using WebAPI.Application.DTOs;
using WebAPI.Infrastructures.Persistence;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.Repositories;

namespace WebAPI.Infrastructures.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly MyDbContext dbContext;
        public AuthorRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AuthorModel>> GetAllAuthors(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            return await dbContext.Authors.Skip(skip).Take(pageSize).Select(a => new AuthorModel
            {
                AuthorID = a.AuthorID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Birthday = a.Birthday,
                Country = a.Country
            }).ToListAsync();
        }

        public async Task<AuthorModel> GetAuthorById(int authorId)
        {
            var author = await dbContext.Authors.FindAsync(authorId);
            return author == null ? null : new AuthorModel
            {
                AuthorID = author.AuthorID,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Birthday = author.Birthday,
                Country = author.Country
            };
        }

        public async Task<bool> AddNewAuthor(AuthorModel author)
        {
            Author newAuthor = new();
            newAuthor.FirstName = author.FirstName;
            newAuthor.LastName = author.LastName;
            newAuthor.Birthday = author.Birthday;
            newAuthor.Country = author.Country;
            await dbContext.Authors.AddAsync(newAuthor);
            return await dbContext.Authors.AddAsync(newAuthor) != null;
        }

        public async Task<bool> DeleteAuthor(int authorId)
        {
            var author = await dbContext.Authors.FindAsync(authorId);
            var result = dbContext.Authors.Remove(author);
            return result != null;
        }
        public async Task<IEnumerable<BookModel>> GetAllBooksByAuthor(int authorId)
        {
            return await dbContext.Books.Where(b => b.AuthorID == authorId).Select(b => new BookModel
            {
                BookID = b.BookID,
                ISBN = b.ISBN,
                BookTitle = b.BookTitle,
                Genre = b.Genre,
                Description = b.Description
            }).ToListAsync();
        }
        public async Task<bool> UpdateAuthor(int authorId, UpdateAuthorRequest data)
        {
            var author = await dbContext.Authors.FindAsync(authorId);
            author.FirstName = data.FirstName;
            author.LastName = data.LastName;
            author.Country = data.Country;
            author.Birthday = data.Birthday;
            return author.FirstName != null;
        }
        private IQueryable<AuthorModel> GetAuthors()
        {
            return dbContext.Authors.Select(a => new AuthorModel
            {
                AuthorID = a.AuthorID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Birthday = a.Birthday,
                Country = a.Country
            });
        }
    }
}
