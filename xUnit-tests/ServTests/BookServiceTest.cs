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
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using WebAPI.Services;
using WebAPI.Infrastructure;
using Microsoft.Extensions.Options;
using System.Drawing.Text;
using WebAPI.Repositories.Interfaces;
using WebAPI.Infrastructure.Interfaces;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace xUnit_tests.ServTests
{
    public class BookServiceTest
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        private BookService bookService;
        private readonly BookRepository bookRepository;
        private readonly MyDbContext dbContext;

        public BookServiceTest()
        {
            dbContext = new MyDbContext(new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            bookRepository = new BookRepository(dbContext);
            unitOfWork = new Mock<IUnitOfWork>();
            bookService = new BookService(bookRepository,unitOfWork.Object);
        }

        [Fact]
        public async Task IssueBook_ShouldIssueBook_WhenBookAndUserAreAvailable()
        {
            int bookId = 1;
            int userId = 1;
            DateTime returnDate = DateTime.Now.AddDays(1);
            var book = new BookModel { BookID = bookId, IsAvailable = 1 };
            var user = new UserModel { UserID = userId };
            unitOfWork.Setup(u=>u.Books.GetBookById(bookId)).ReturnsAsync(book);
            unitOfWork.Setup(u=>u.Users.GetUserById(userId)).ReturnsAsync(user);
            unitOfWork.Setup(u => u.Books.UpdateBook(bookId, It.IsAny<UpdateBookRequest>())).ReturnsAsync(true);
            unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            var result = await bookService.IssueBook(bookId, userId, returnDate);
            Assert.True(result);
            unitOfWork.Verify(u=>u.Books.UpdateBook(bookId, It.IsAny<UpdateBookRequest>()), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task IssueBook_ShouldThrowException_IfBookIsUnavailable()
        {
            var bookId = 1;
            var userId = 1;
            var returnDate = DateTime.Now.AddDays(14);
            var book = new BookModel { BookID = bookId, IsAvailable = 0 };
            unitOfWork.Setup(u => u.Books.GetBookById(bookId)).ReturnsAsync(book);
            var exception = await Assert.ThrowsAsync<Exception>(() => bookService.IssueBook(bookId, userId, returnDate));
            Assert.Equal("The book is unavailable", exception.Message);
            unitOfWork.Verify(u => u.Books.UpdateBook(It.IsAny<int>(), It.IsAny<UpdateBookRequest>()), Times.Never);
        }

        [Fact]
        public async Task IssueBook_ShouldThrowException_IfUserNotFound()
        {
            var bookId = 1;
            var userId = 1;
            var returnDate = DateTime.Now.AddDays(14);
            var book = new BookModel { BookID = bookId, IsAvailable = 1 };
            unitOfWork.Setup(u => u.Books.GetBookById(bookId)).ReturnsAsync(book);
            unitOfWork.Setup(u => u.Users.GetUserById(userId)).ReturnsAsync((UserModel)null);
            var exception = await Assert.ThrowsAsync<Exception>(() => bookService.IssueBook(bookId, userId, returnDate));
            Assert.Equal("User not found", exception.Message);
            unitOfWork.Verify(u => u.Books.UpdateBook(It.IsAny<int>(), It.IsAny<UpdateBookRequest>()), Times.Never);
        }

        [Fact]
        public async Task ReturnBook_ShouldThrowException_IfBookIsAlreadyReturned()
        {
            var bookId = 1;
            var book = new BookModel { BookID = bookId, IsAvailable = 1 };
            unitOfWork.Setup(u => u.Books.GetBookById(bookId)).ReturnsAsync(book);
            var exception = await Assert.ThrowsAsync<Exception>(() => bookService.ReturnBook(bookId));
            Assert.Equal("The book is already returned or not found", exception.Message);
            unitOfWork.Verify(u => u.Books.UpdateBook(It.IsAny<int>(), It.IsAny<UpdateBookRequest>()), Times.Never);
        }

        [Fact]
        public async Task ReturnBook_ShouldThrowException_IfBookIsUnavailable()
        {
            var bookId = 1;
            unitOfWork.Setup(u=>u.Books.GetBookById(bookId)).ReturnsAsync((BookModel)null);
            var exception = await Assert.ThrowsAsync<Exception>(() => bookService.ReturnBook(bookId));
            Assert.Equal("The book is already returned or not found", exception.Message);
            unitOfWork.Verify(u => u.Books.UpdateBook(It.IsAny<int>(), It.IsAny<UpdateBookRequest>()), Times.Never);
        }

        [Fact]
        public async Task ReturnBook_ShouldReturnBook_WhenItIsValid()
        {
            var bookId = 1;
            var book = new BookModel
            {
                BookID = bookId,
                IsAvailable = 0,
                PickUpTime = DateTime.Now,
                ReturnTime = DateTime.Now.AddDays(1),
                UserID = 1
            };
            unitOfWork.Setup(u=>u.Books.GetBookById(bookId)).ReturnsAsync(book);
            unitOfWork.Setup(u => u.Books.UpdateBook(bookId, It.IsAny<UpdateBookRequest>())).ReturnsAsync(true);
            unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            var result = await bookService.ReturnBook(bookId);
            Assert.True(result);
            unitOfWork.Verify(u=>u.Books.UpdateBook(bookId,It.IsAny<UpdateBookRequest>()),Times.Once);
            unitOfWork.Verify(u=>u.SaveChangesAsync(), Times.Once);
        }

    }
}
