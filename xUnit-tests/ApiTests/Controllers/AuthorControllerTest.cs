using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.API;
using WebAPI.API.Controllers;
using WebAPI.Application.DTOs;
using WebAPI.Application.UseCases.Authors;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.UnitOfWork;
using WebAPI.Infrastructure.Exceptions;

namespace xUnit_tests.ApiTests.Controllers
{
    public class AuthorControllerTest
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IValidator<AuthorModel>> mockAuthorModelValidator;
        private readonly Mock<IValidator<UpdateAuthorRequest>> mockUpdateAuthorRequestValidator;
        private readonly Mock<GetAuthorsUseCase> mockGetAuthorsService;
        private readonly Mock<AddNewAuthorUseCase> mockAddNewAuthorService;
        private readonly Mock<UpdateAuthorUseCase> mockUpdateAuthorService;
        private readonly Mock<DeleteAuthorUseCase> mockDeleteAuthorService;
        private readonly Mock<GetAllBooksByAuthorUseCase> mockGetAllBooksByAuthorService;
        private readonly AuthorController controller;
        public AuthorControllerTest()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockAuthorModelValidator = new Mock<IValidator<AuthorModel>>();
            mockUpdateAuthorRequestValidator = new Mock<IValidator<UpdateAuthorRequest>>();
            mockGetAuthorsService = new Mock<GetAuthorsUseCase>(mockUnitOfWork.Object);
            mockAddNewAuthorService = new Mock<AddNewAuthorUseCase>(mockUnitOfWork.Object);
            mockUpdateAuthorService = new Mock<UpdateAuthorUseCase>(mockUnitOfWork.Object);
            mockDeleteAuthorService = new Mock<DeleteAuthorUseCase>(mockUnitOfWork.Object);
            mockGetAllBooksByAuthorService = new Mock<GetAllBooksByAuthorUseCase>(mockUnitOfWork.Object);
            controller = new AuthorController(mockAddNewAuthorService.Object, mockDeleteAuthorService.Object,
                mockGetAllBooksByAuthorService.Object, mockGetAuthorsService.Object, mockUpdateAuthorService.Object);
        }

        [Fact]
        public async Task GetAllAuthors_ReturnsOkResult_WithListOfAuthors()
        {
            int pageNubmer = 1;
            int pageSize = 10;

            var authors = new List<AuthorModel>
            {
                new AuthorModel { AuthorID = 1, FirstName = "test1", LastName="test1"},
                new AuthorModel { AuthorID = 2, FirstName = "test2", LastName="test2"}
            };
            mockGetAuthorsService.Setup(service => service.GetAllAuthors(pageNubmer, pageSize)).ReturnsAsync(authors);
            var result = await controller.GetAllAuthors(pageNubmer, pageSize);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<AuthorModel>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllAuthors_ThrowsException_ReturnsInternalServerError()
        {
            int pageNubmer = 1;
            int pageSize = 10;

            mockGetAuthorsService.Setup(service => service.GetAllAuthors(pageNubmer, pageSize))
                .ThrowsAsync(new Exception("Something went wrong"));

            var result = await controller.GetAllAuthors(pageNubmer, pageSize);
            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
            Assert.Equal("Something went wrong", objResult.Value);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsOkResult_WithAuthorModel()
        {
            var author = new AuthorModel
            {
                AuthorID = 1,
                FirstName = "test1",
                LastName = "test1"
            };
            mockGetAuthorsService.Setup(service => service.GetAuthorById(1)).ReturnsAsync(author);
            var result = await controller.GetAuthorById(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<AuthorModel>(okResult.Value);
            Assert.Equal("test1", returnValue.FirstName);
        }

        [Fact]
        public async Task GetAuthorById_ThrowsException_ReturnsInternalServerError()
        {
            mockGetAuthorsService.Setup(service => service.GetAuthorById(1))
                .ThrowsAsync(new EntityNotFoundException("Author", 1));
            var result = await controller.GetAuthorById(1);
            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
            Assert.Equal("Author with key 1 was not found", objResult.Value);
        }

        [Fact]
        public async Task AddNewAuthor_ValidAuthor_ReturnsOkResult()
        {
            var author = new AuthorModel
            {
                AuthorID = 1,
                FirstName = "test1",
                LastName = "test1"
            };

            mockAuthorModelValidator.Setup(v => v.ValidateAsync(author, default)).ReturnsAsync(new ValidationResult());

            mockAddNewAuthorService.Setup(service => service.AddNewAuthor(author)).ReturnsAsync(1);

            var result = await controller.AddNewAuthor(author, mockAuthorModelValidator.Object);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<int>(okResult.Value);
            Assert.Equal(1, returnValue);
        }

        [Fact]
        public async Task AddNewAuthor_InvalidAuthor_ReturnsBadRequest()
        {
            var author = new AuthorModel
            {
                AuthorID = 1,
                FirstName = "",
                LastName = "test1"
            };
            mockAuthorModelValidator.Setup(v => v.ValidateAsync(author, default))
                .ThrowsAsync(new ValidationException("Firstname length can't be less than 1."));
            var result = await controller.AddNewAuthor(author, mockAuthorModelValidator.Object);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Firstname length can't be less than 1.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateAuthor_ValidUpdateAuthorRequest_ReturnsOkResult()
        {
            var author = new UpdateAuthorRequest
            {
                FirstName = "newTest1",
                LastName = "newTest1"
            };
            mockUpdateAuthorRequestValidator.Setup(v => v.ValidateAsync(author, default)).ReturnsAsync(new ValidationResult());
            mockUpdateAuthorService.Setup(service => service.UpdateAuthor(1, author)).ReturnsAsync(true);
            var result = await controller.UpdateAuthor(1, author, mockUpdateAuthorRequestValidator.Object);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task UpdateAuthor_InvalidUpdateAuthorRequest_ReturnsOkResult()
        {
            var author = new UpdateAuthorRequest
            {
                FirstName = "",
                LastName = "test1"
            };
            mockUpdateAuthorRequestValidator.Setup(v => v.ValidateAsync(author, default))
                .ThrowsAsync(new ValidationException("Firstname length can't be less than 1."));
            var result = await controller.UpdateAuthor(1, author, mockUpdateAuthorRequestValidator.Object);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Firstname length can't be less than 1.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteAuthor_ValidId_ReturnsOkResult()
        {
            int authorId = 1;
            mockDeleteAuthorService.Setup(s => s.DeleteAuthor(authorId)).ReturnsAsync(1);
            var result = await controller.DeleteAuthor(authorId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, okResult.Value);
        }

        [Fact]
        public async Task DeleteAuthor_Invalid_ReturnsBadRequest()
        {
            int authorId = 1000;
            mockDeleteAuthorService.Setup(s => s.DeleteAuthor(authorId))
                .ThrowsAsync(new EntityNotFoundException("Author", authorId));
            var result = await controller.DeleteAuthor(authorId);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Author with key 1000 was not found.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAllBooksByAuthor_ValidId_ReturnOkResult_WithListOfBooks()
        {
            int authorId = 1;
            var books = new List<BookModel>
            {
            new BookModel { BookID = 1, ISBN = "111-1-1111-1111-1", BookTitle = "test1", Genre = "test1" },
            new BookModel { BookID = 2, ISBN = "222-2-2222-2222-2", BookTitle = "test2", Genre = "test2" }
            };
            mockGetAllBooksByAuthorService.Setup(s => s.GetAllBooksByAuthor(authorId)).ReturnsAsync(books);
            var result = await controller.GetAllBooksByAuthor(authorId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<BookModel>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllBooksByAuthor_InvalidId_ReturnBadRequest()
        {
            int authorId = 1000;
            mockGetAllBooksByAuthorService.Setup(s => s.GetAllBooksByAuthor(authorId))
            .ThrowsAsync(new EntityNotFoundException("Books by author", authorId));
            var result = await controller.GetAllBooksByAuthor(authorId);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Books by author with key 1000 was not found", badRequestResult.Value);
        }


    }
}