using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Authors;


namespace WebAPI.API.Controllers
{
    [ApiController]
    [Route("authorController")]
    public class AuthorController : ControllerBase
    {
        private readonly IAddNewAuthorService addNewAuthorService;
        private readonly IDeleteAuthorService deleteAuthorService;
        private readonly IGetAllBooksByAuthorService getAllBooksByAuthorService;
        private readonly IGetAuthorsService getAuthorsService;
        private readonly IUpdateAuthorService updateAuthorService;

        public AuthorController(IAddNewAuthorService addNewAuthorService, IDeleteAuthorService deleteAuthorService,
            IGetAllBooksByAuthorService getAllBooksByAuthorService, IGetAuthorsService getAuthorsService,
            IUpdateAuthorService updateAuthorService)
        {
            this.addNewAuthorService = addNewAuthorService;
            this.deleteAuthorService = deleteAuthorService;
            this.getAllBooksByAuthorService = getAllBooksByAuthorService;
            this.getAuthorsService = getAuthorsService;
            this.updateAuthorService = updateAuthorService;
        }

        
        [HttpGet("getAllAuthors")]
        [Authorize(Policy = "UserOnly")] //Доступ для всех ролей, т.к. добавлена иерархия ролей в RoleHierarchyHandler
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllAuthors(int pageNumber, int pageSize)
        {
            return Ok(await getAuthorsService.GetAllAuthors(pageNumber, pageSize));
        }

        [HttpGet("getAuthorById")]
        [Authorize(Policy = "UserOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAuthorById(int authorId)
        {
            return Ok(await getAuthorsService.GetAuthorById(authorId));
        }

        [HttpPost("addNewAuthor")]
        [Authorize(Policy = "LibrarianOnly")] //Доступ для RoleName = Librarian и выше
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewAuthor(AuthorModel author)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await addNewAuthorService.AddNewAuthor(author));
        }

        [HttpPost("updateAuthor")]
        [Authorize(Policy = "LibrarianOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAuthor(int authorId, UpdateAuthorRequest data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await updateAuthorService.UpdateAuthor(authorId, data));
        }

        [HttpDelete("deleteAuthor")]
        [Authorize(Policy = "LibrarianOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAuthor(int authorId)
        {
            return Ok(await deleteAuthorService.DeleteAuthor(authorId));
        }

        [HttpGet("getAllBooksByAuthor")]
        [Authorize(Policy = "UserOnly")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllBooksByAuthor(int authorId)
        {
            return Ok(await getAllBooksByAuthorService.GetAllBooksByAuthor(authorId));
        }
    }
}
