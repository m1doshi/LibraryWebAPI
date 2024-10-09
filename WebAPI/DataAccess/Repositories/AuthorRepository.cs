using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.DataAccess.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly MyDbContext dbContext;
        public AuthorRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Author>> GetAllAuthors(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            return await dbContext.Authors.Skip(skip).Take(pageSize).ToListAsync();
        }

        public async Task<Author> GetAuthorById(int authorId)
        {
            return await dbContext.Authors.FindAsync(authorId);
        }

        public async Task<bool> AddNewAuthor(Author newAuthor)
        {
            await dbContext.Authors.AddAsync(newAuthor);
            return true;
        }

        public async Task<bool> DeleteAuthor(int authorId)
        {
            var author = await dbContext.Authors.FindAsync(authorId);
            dbContext.Authors.Remove(author);
            return true;
        }
        public async Task<IEnumerable<Book>> GetAllBooksByAuthor(int authorId)
        {
            return await dbContext.Books.Where(b=>b.AuthorID == authorId).ToListAsync();
        }
        public async Task<bool> UpdateAuthor(Author author)
        {
            dbContext.Authors.Update(author);
            return true;
        }
    }
}
