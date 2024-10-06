using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Core.DTOs;
using WebAPI.Application.UseCases.Books;
using WebAPI.DataAccess.Exceptions;

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
            var result = await getBooksService.GetBookById(bookId);
            if(result == null)
            {
                return BadRequest(new EntityNotFoundException("Book", bookId));
            }
            return Ok(result);
        }

        [HttpGet("getBookByISBN")]
        [Authorize(Policy = "UserOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBookByISBN(string isbn)
        {
            var result = await getBooksService.GetBookByISBN(isbn);
            if (result == null)
            {
                return BadRequest(new EntityNotFoundException("Book", isbn));
            }
            return Ok(result);
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
            var result = await deleteBookService.DeleteBook(bookId);
            if(result == 0)
            {
                return BadRequest(new EntityNotFoundException("Book", bookId));
            }
            return Ok(result);
        }

        [HttpPost("updateImage")]
        [Authorize(Policy = "LibrarianOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateImage(int bookId, IFormFile image)
        {
            var result = await updateImageService.UpdateImage(bookId, image);
            if(result == 0)
            {
                return BadRequest(new EntityNotFoundException("Book", bookId));
            }
            return Ok(result);
        }

        [HttpPost("issueBook")]
        [Authorize(Policy = "UserOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> IssueBook(IssueBookRequest request)
        {
            try
            {
                var result = await bookShareService.IssueBook(request);
                if(!result)
                {
                    return BadRequest("Unable to issue the book.");
                }
                return Ok(result);
            }
            catch (BusinessRuleViolationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("returnBook")]
        [Authorize(Policy = "UserOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ReturnBook(int bookId)
        {
            try
            {
                var result = await bookShareService.ReturnBook(bookId);
                if (!result)
                {
                    return BadRequest("Unable to return the book.");
                }
                return Ok(result);
            }
            catch (BusinessRuleViolationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
