using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("bookController")]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;
        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet("getAllBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllBooks(int pageNumber, int pageSize)
        {
            return Ok(await bookService.GetAllBooks(pageNumber,pageSize));
        }

        [HttpGet("getBookById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBookById(int bookId)
        {
            return Ok(await bookService.GetBookById(bookId));
        }

        [HttpGet("getBookByISBN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBookByISBN(string isbn)
        {
            return Ok(await bookService.GetBookByISBN(isbn));
        }

        [HttpPost("addNewBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewBook(BookModel book)
        {
            return Ok(await bookService.AddNewBook(book));
        }

        [HttpPost("updateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateBook(int bookId, UpdateBookRequest data)
        {
            return Ok (await  bookService.UpdateBook(bookId, data));
        }

        [HttpDelete("deleteBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            return Ok(await bookService.DeleteBook(bookId));
        }

        [HttpPost("updateImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateImage(int bookId, IFormFile image)
        {
            return Ok(await bookService.UpdateImage(bookId, image));
        }

        [HttpPost("issueBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> IssueBook(int bookId, int userId, DateTime returnDate)
        {
            return Ok(await bookService.IssueBook(bookId, userId, returnDate));
        }

        [HttpPost("returnBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ReturnBook(int bookId)
        {
            return Ok(await bookService.ReturnBook(bookId));
        }
    }
}
