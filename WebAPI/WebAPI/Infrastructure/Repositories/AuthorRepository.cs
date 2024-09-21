﻿using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.DTOs;
using WebAPI.Infrastructures.Persistence;
using WebAPI.Domain.Entities;

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
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 1 : pageSize;

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
            return await dbContext.Authors.Where(a => a.AuthorID == authorId).Select(a => new AuthorModel
            {
                AuthorID = a.AuthorID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Birthday = a.Birthday,
                Country = a.Country
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> AddNewAuthor(AuthorModel author)
        {
            Author newAuthor = new();
            //newAuthor.AuthorID = author.AuthorID;
            newAuthor.FirstName = author.FirstName;
            newAuthor.LastName = author.LastName;
            newAuthor.Birthday = author.Birthday;
            newAuthor.Country = author.Country;
            await dbContext.Authors.AddAsync(newAuthor);
            return true;
        }

        public async Task<bool> DeleteAuthor(int authorId)
        {
            var author = await dbContext.Authors.FindAsync(authorId);
            if (author == null) return false;
            dbContext.Remove(author);
            return true;
        }
        public async Task<IEnumerable<BookModel>> GetAllBooksByAuthor(int authorId)
        {
            return await dbContext.Books.Where(b => b.AuthorID == authorId).Select(b => new BookModel
            {
                ISBN = b.ISBN,
                BookTitle = b.BookTitle,
                Genre = b.Genre,
                Description = b.Description
            }).ToListAsync();
        }
        public async Task<bool> UpdateAuthor(int authorId, UpdateAuthorRequest data)
        {
            var author = await dbContext.Authors.Where(a => a.AuthorID == authorId).FirstOrDefaultAsync();
            if (author == null) return false;
            author.FirstName = data.FirstName == null ? author.FirstName : data.FirstName;
            author.LastName = data.LastName == null ? author.LastName : data.LastName;
            author.Country = data.Country == null ? author.Country : data.Country;
            author.Birthday = data.Birthday.ToString() == null ? author.Birthday : data.Birthday;
            return true;
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
