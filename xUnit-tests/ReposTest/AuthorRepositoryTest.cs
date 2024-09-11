using Microsoft.EntityFrameworkCore;
using Xunit;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Entities;
using WebAPI.Database;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace xUnit_tests.ReposTest
{
    public class AuthorRepositoryTest
    {
        private MyDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new MyDbContext(options);
        }

        [Fact]
        public async Task GetAllAuthors_ShouldReturnAllAuthors()
        {
            var context = GetInMemoryDbContext();
            var repository = new AuthorRepository(context);

            var author = new Author
            {
                AuthorID = 1,
                FirstName = "test1",
                LastName = "test1",
                Country = "test1",
                Birthday = new DateOnly(1900, 8, 28)
            };
            await context.Authors.AddAsync(author);
            await context.SaveChangesAsync();

            var authors = await repository.GetAllAuthors(1, 1);
            Assert.Single(authors);
        }

        [Fact]
        public async Task GetAuthorById_ShouldReturnNull_WhenAuthorDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var repository = new AuthorRepository(context);

            var author = await repository.GetAuthorById(10);
            Assert.Null(author);
        }

        [Fact]
        public async Task GetAuthorById_ShouldReturnAuthor_WhenAuthorExists()
        {
            var context = GetInMemoryDbContext();
            var repository = new AuthorRepository(context);

            var newAuthor = new Author
            {
                AuthorID = 1,
                FirstName = "test1",
                LastName = "test1",
                Country = "test1",
                Birthday = new DateOnly(1900, 8, 28)
            };
            await context.Authors.AddAsync(newAuthor);
            await context.SaveChangesAsync();

            var author = await repository.GetAuthorById(1);
            Assert.NotNull(author);
            Assert.Equal(1, author.AuthorID);
            Assert.Equal("test1", author.FirstName);
            Assert.Equal(new DateOnly(1900, 8, 28), author.Birthday);
        }

        [Fact]
        public async Task AddNewAuthor_ShouldAddNewAuthor()
        {
            var context = GetInMemoryDbContext();
            var repository = new AuthorRepository(context);

            var newAuthor = new AuthorModel
            {
                AuthorID = 1,
                FirstName = "test1",
                LastName = "test1",
                Country = "test1",
                Birthday = new DateOnly(1900, 8, 28)
            };
            await repository.AddNewAuthor(newAuthor);
            await context.SaveChangesAsync();

            var author = await context.Authors.FindAsync(1);
            Assert.NotNull(author);
            Assert.Equal(1, author.AuthorID);
            Assert.Equal("test1", author.LastName);
        }

        [Fact]
        public async Task DeleteAuthor_ShouldDeleteAuthors()
        {
            var context = GetInMemoryDbContext();
            var repository = new AuthorRepository(context);
            var newAuthor = new Author
            {
                AuthorID = 1,
                FirstName = "test1",
                LastName = "test1",
                Country = "test1",
                Birthday = new DateOnly(1900, 8, 28)
            };
            await context.Authors.AddAsync(newAuthor);
            await context.SaveChangesAsync();

            await repository.DeleteAuthor(newAuthor.AuthorID);
            await context.SaveChangesAsync();
            var deletedAuthor = await context.Authors.FindAsync(1);
            Assert.Null(deletedAuthor);
        }

        [Fact]
        public async Task UpdateAuthor_ShouldUpdateAuthor_WhenAuthorExists()
        {
            var context = GetInMemoryDbContext();
            var repository = new AuthorRepository(context);
            var newAuthor = new Author
            {
                AuthorID = 1,
                FirstName = "test1",
                LastName = "test1",
                Country = "test1",
                Birthday = new DateOnly(1900, 8, 28)
            };
            await context.Authors.AddAsync(newAuthor);
            await context.SaveChangesAsync();
            var newData = new UpdateAuthorRequest { FirstName = "test001", Country = "test001" };
            await repository.UpdateAuthor(1, newData);
            await context.SaveChangesAsync();
            var updatedAuthor = await context.Authors.FindAsync(1);
            Assert.Equal("test001", updatedAuthor.FirstName);
            Assert.Equal("test001", updatedAuthor.Country);
        }

        [Fact]
        public async Task UpdateAuthor_ShouldReturnNull_WhenAuthorDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var repository = new AuthorRepository(context);

            var newData = new UpdateAuthorRequest { FirstName = "test001", Country = "test001" };
            await repository.UpdateAuthor(10, newData);
            await context.SaveChangesAsync();

            var updatedAuthor = await context.Authors.FindAsync(10);
            Assert.Null(updatedAuthor);
        }

        [Fact]
        public async Task GetAllBooksByAuthor_ShouldReturnAllBooksByAuthor_WhenAuthorExists()
        {
            var context = GetInMemoryDbContext();
            var repository = new AuthorRepository(context);
            var newAuthor = new Author
            {
                AuthorID = 1,
                FirstName = "test1",
                LastName = "test1",
                Country = "test1",
                Birthday = new DateOnly(1900, 8, 28)
            };
            var newBook = new Book
            {
                BookID = 1,
                AuthorID = 1,
                BookTitle = "test1",
                Description = "test1",
                Genre = "test1",
                ISBN = "111"
            };
            await context.Books.AddAsync(newBook);
            await context.SaveChangesAsync();
            await context.Authors.AddAsync(newAuthor);
            await context.SaveChangesAsync();

            var bookList = await repository.GetAllBooksByAuthor(1);

            Assert.NotNull(bookList);
            Assert.Single(bookList);
        }

        [Fact]
        public async Task GetAllBooksByAuthor_ShouldReturnNull_WhenAuthorDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var repository = new AuthorRepository(context);
            var bookList = await repository.GetAllBooksByAuthor(1);
            Assert.Empty(bookList);
        }

    }
}
