using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Book;

namespace WebAPI.API.Controllers
{
    [ApiController]
    [Route("bookController")]
    public class BookController : ControllerBase
    {
        private readonly IAddNewBookService addNewBookService;
        private readonly IBookShareService bookShareService;
        private readonly IDeleteBookService deleteBookService;
        private readonly IGetBooksService getBooksService;
        private readonly IUpdateBookService updateBookService;
        private readonly IUpdateImageService updateImageService;
        public BookController(IAddNewBookService addNewBookService, IBookShareService bookShareService,
            IDeleteBookService deleteBookService, IGetBooksService getBooksService, IUpdateBookService updateBookService,
            IUpdateImageService updateImageService)
        {
           this.addNewBookService = addNewBookService;
           this.bookShareService = bookShareService;
           this.deleteBookService = deleteBookService;
           this.getBooksService = getBooksService;
           this.updateBookService = updateBookService;
           this.updateImageService = updateImageService;
        }

        [HttpGet("getAllBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllBooks(int pageNumber, int pageSize)
        {
            return Ok(await getBooksService.GetAllBooks(pageNumber, pageSize));
        }

        [HttpGet("getBookById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBookById(int bookId)
        {
            return Ok(await getBooksService.GetBookById(bookId));
        }

        [HttpGet("getBookByISBN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetBookByISBN(string isbn)
        {
            return Ok(await getBooksService.GetBookByISBN(isbn));
        }

        [HttpPost("addNewBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewBook(BookModel book)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await addNewBookService.AddNewBook(book));
        }

        [HttpPost("updateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateBook(int bookId, UpdateBookRequest data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await updateBookService.UpdateBook(bookId, data));
        }

        [HttpDelete("deleteBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            return Ok(await deleteBookService.DeleteBook(bookId));
        }

        [HttpPost("updateImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateImage(int bookId, IFormFile image)
        {
            return Ok(await updateImageService.UpdateImage(bookId, image));
        }

        [HttpPost("issueBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> IssueBook(int bookId, int userId, DateTime returnDate)
        {
            return Ok(await bookShareService.IssueBook(bookId, userId, returnDate));
        }

        [HttpPost("returnBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ReturnBook(int bookId)
        {
            return Ok(await bookShareService.ReturnBook(bookId));
        }
    }
}
