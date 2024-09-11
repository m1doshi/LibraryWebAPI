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
using Microsoft.AspNetCore.Http;
using Moq;

namespace xUnit_tests.ReposTest
{
    public class BookRepositoryTest
    {
        private MyDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new MyDbContext(options);
        }

        [Fact]
        public async Task GetAllBooks_ShouldReturnAllBooks()
        {
            var context = GetInMemoryDbContext();
            var repository = new BookRepository(context);

            var book = new Book
            {
                BookID = 1,
                AuthorID = 1,
                BookTitle = "test1",
                Description = "test1",
                Genre = "test1",
                ISBN = "111"
            };
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var books = await repository.GetAllBooks(1, 1);
            Assert.Single(books);

        }

        [Fact]
        public async Task AddNewBook_ShouldAddNewBook()
        {
            var context = GetInMemoryDbContext();
            var repository = new BookRepository(context);

            var newBook = new BookModel
            {
                BookID = 1,
                AuthorID = 1,
                BookTitle = "test1",
                Description = "test1",
                Genre = "test1",
                ISBN = "111"
            };
            await repository.AddNewBook(newBook);
            await context.SaveChangesAsync();

            var addedBook = await context.Books.FirstOrDefaultAsync(b => b.BookTitle == "test1");

            Assert.NotNull(addedBook);
            Assert.Equal("test1", addedBook.BookTitle);
        }

        [Fact]
        public async Task GetBookById_ShouldReturnNull_WhenBookDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var repository = new BookRepository(context);

            var book = await repository.GetBookById(50);
            Assert.Null(book);
        }

        [Fact]
        public async Task GetBookById_ShouldReturnBook_WhenBookExists()
        {
            var context = GetInMemoryDbContext();
            var repository = new BookRepository(context);

            var newBook = new Book
            {
                BookID = 1,
                AuthorID = 1,
                BookTitle = "test1",
                Description = "test1",
                Genre = "test1",
                ISBN = "111"
            };
            context.Books.Add(newBook);
            await context.SaveChangesAsync();

            var book = await repository.GetBookById(1);
            Assert.Equal(1, book.BookID);
            Assert.Equal("test1", book.BookTitle);
        }

        [Fact]
        public async Task GetBookByISBN_ShouldReturnNull_WhenBookDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var repository = new BookRepository(context);

            var book = await repository.GetBookByISBN("9999");
            Assert.Null(book);
        }

        [Fact]
        public async Task GetBookByISBN_ShouldReturnBook_WhenBookExists()
        {
            var context = GetInMemoryDbContext();
            var repository = new BookRepository(context);

            var newBook = new Book
            {
                BookID = 1,
                AuthorID = 1,
                BookTitle = "test1",
                Description = "test1",
                Genre = "test1",
                ISBN = "111"
            };
            context.Books.Add(newBook);
            await context.SaveChangesAsync();

            var book = await repository.GetBookByISBN("111");
            Assert.Equal("111", book.ISBN);
            Assert.Equal("test1", book.Genre);
        }

        [Fact]
        public async Task UpdateBook_ShouldUpdateBook_WhenBookExists()
        {

            var context = GetInMemoryDbContext();
            var repository = new BookRepository(context);

            var newBook = new Book
            {
                BookID = 1,
                AuthorID = 1,
                BookTitle = "test1",
                Description = "test1",
                Genre = "test1",
                ISBN = "111"
            };
            context.Books.Add(newBook);
            await context.SaveChangesAsync();

            var newData = new UpdateBookRequest { ISBN = "001", Genre = "test001" };
            await repository.UpdateBook(1, newData);
            await context.SaveChangesAsync();
            var updatedBook = await context.Books.FindAsync(1);

            Assert.NotNull(updatedBook);
            Assert.Equal("001", updatedBook.ISBN);
            Assert.Equal("test001", updatedBook.Genre);
            Assert.Equal("test1", updatedBook.BookTitle);
        }

        [Fact]
        public async Task UpdateBook_ShouldReturnNull_WhenBookDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var repository = new BookRepository(context);

            var newData = new UpdateBookRequest { ISBN = "555", Genre = "test555" };
            await repository.UpdateBook(55, newData);
            await context.SaveChangesAsync();

            var book = await context.Books.FindAsync(55);

            Assert.Null(book);
        }

        [Fact]
        public async Task DeleteBook_ShouldDeleteBook()
        {
            var context = GetInMemoryDbContext();
            var repository = new BookRepository(context);

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

            await repository.DeleteBook(newBook.BookID);
            await context.SaveChangesAsync();

            var deletedBook = await context.Books.FindAsync(1);

            Assert.Null(deletedBook);
        }

        [Fact]
        public async Task UpdateImage_ShouldUpdateImage_WhenBookExists()
        {

            var context = GetInMemoryDbContext();
            var bookRepository = new BookRepository(context);

            var book = new Book
            {
                BookID = 1,
                AuthorID = 1,
                BookTitle = "test1",
                Description = "test1",
                Genre = "test1",
                ISBN = "111",
                Image = null
            };
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();

            var fileMock = new Mock<IFormFile>();
            var content = "image file";
            var fileName = "test.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);

            await bookRepository.UpdateImage(1, fileMock.Object);
            await context.SaveChangesAsync();

            var updatedBook = await context.Books.FindAsync(1);
            Assert.NotNull(updatedBook);
            Assert.NotNull(updatedBook.Image);
        }

        [Fact]
        public async Task UpdateImage_ShouldReturnNull_WhenBookDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var bookRepository = new BookRepository(context);

            var fileMock = new Mock<IFormFile>();
            var content = "image file";
            var fileName = "test.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);

            await bookRepository.UpdateImage(10, fileMock.Object);
            await context.SaveChangesAsync();
            var book = await context.Books.FindAsync(10);
            Assert.Null(book);
        }

    }
}
