using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebAPI.Application.DTOs;
using WebAPI.Application.UseCases.Books;
using WebAPI.Application.Validators;
using WebAPI.Domain.Entities;

namespace WebAPI.API.Controllers
{
    [ApiController]
    [Route("bookController")]
    public class BookController : ControllerBase
    {
        private readonly AddNewBookUseCase addNewBookService;
        private readonly BookShareUseCase bookShareService;
        private readonly DeleteBookUseCase deleteBookService;
        private readonly GetBooksUseCase getBooksService;
        private readonly UpdateBookUseCase updateBookService;
        private readonly UpdateImageUseCase updateImageService;
        public BookController(AddNewBookUseCase addNewBookService, BookShareUseCase bookShareService,
            DeleteBookUseCase deleteBookService, GetBooksUseCase getBooksService, UpdateBookUseCase updateBookService,
            UpdateImageUseCase updateImageService)
        {
           this.addNewBookService = addNewBookService;
           this.bookShareService = bookShareService;
           this.deleteBookService = deleteBookService;
           this.getBooksService = getBooksService;
           this.updateBookService = updateBookService;
           this.updateImageService = updateImageService;
        }

        [HttpGet("getAllBooks")]
        [Authorize(Policy = "UserOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllBooks(int pageNumber, int pageSize)
        {
            return Ok(await getBooksService.GetAllBooks(pageNumber, pageSize));
        }

        [HttpGet("getBookById")]
        [Authorize(Policy = "UserOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBookById(int bookId)
        {
            return Ok(await getBooksService.GetBookById(bookId));
        }

        [HttpGet("getBookByISBN")]
        [Authorize(Policy = "UserOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBookByISBN(string isbn)
        {
            return Ok(await getBooksService.GetBookByISBN(isbn));
        }

        [HttpPost("addNewBook")]
        [Authorize(Policy = "LibrarianOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewBook([FromBody] BookModel book, [FromServices] IValidator<BookModel> validator)
        {
            var validationResult = await validator.ValidateAsync(book);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return Ok(await addNewBookService.AddNewBook(book));
        }

        [HttpPost("updateBook")]
        [Authorize(Policy = "LibrarianOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateBook(int bookId, [FromBody] UpdateBookRequest data, [FromServices] IValidator<UpdateBookRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(data);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return Ok(await updateBookService.UpdateBook(bookId, data));
        }

        [HttpDelete("deleteBook")]
        [Authorize(Policy = "LibrarianOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            return Ok(await deleteBookService.DeleteBook(bookId));
        }

        [HttpPost("updateImage")]
        [Authorize(Policy = "LibrarianOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateImage(int bookId, IFormFile image)
        {
            return Ok(await updateImageService.UpdateImage(bookId, image));
        }

        [HttpPost("issueBook")]
        [Authorize(Policy = "UserOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> IssueBook(int bookId, int userId, DateTime returnDate)
        {
            return Ok(await bookShareService.IssueBook(bookId, userId, returnDate));
        }

        [HttpPost("returnBook")]
        [Authorize(Policy = "UserOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ReturnBook(int bookId)
        {
            return Ok(await bookShareService.ReturnBook(bookId));
        }
    }
}
