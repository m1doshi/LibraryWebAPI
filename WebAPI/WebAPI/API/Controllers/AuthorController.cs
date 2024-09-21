using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services;


namespace WebAPI.API.Controllers
{
    [ApiController]
    [Route("authorController")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService authorService;

        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpGet("getAllAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllAuthors(int pageNumber, int pageSize)
        {
            return Ok(await authorService.GetAllAuthors(pageNumber, pageSize));
        }
        [HttpGet("getAuthorById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAuthorById(int authorId)
        {
            return Ok(await authorService.GetAuthorById(authorId));
        }

        [HttpPost("addNewAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewAuthor(AuthorModel author)
        {
            return Ok(await authorService.AddNewAuthor(author));
        }

        [HttpPost("updateAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAuthor(int authorId, UpdateAuthorRequest data)
        {
            return Ok(await authorService.UpdateAuthor(authorId, data));
        }

        [HttpDelete("deleteAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAuthor(int authorId)
        {
            return Ok(await authorService.DeleteAuthor(authorId));
        }

        [HttpGet("getAllBooksByAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllBooksByAuthor(int authorId)
        {
            return Ok(await authorService.GetAllBooksByAuthor(authorId));
        }
    }
}
