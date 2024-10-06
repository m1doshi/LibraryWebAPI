using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Core.DTOs;
using WebAPI.Application.UseCases.Authors;
using WebAPI.DataAccess.Exceptions;

namespace WebAPI.API.Controllers
{
    [ApiController]
    [Route("authorController")]
    public class AuthorController : ControllerBase
    {
        private readonly AddNewAuthorUseCase addNewAuthorService;
        private readonly DeleteAuthorUseCase deleteAuthorService;
        private readonly GetAllBooksByAuthorUseCase getAllBooksByAuthorService;
        private readonly GetAuthorsUseCase getAuthorsService;
        private readonly UpdateAuthorUseCase updateAuthorService;

        public AuthorController(AddNewAuthorUseCase addNewAuthorService, DeleteAuthorUseCase deleteAuthorService,
            GetAllBooksByAuthorUseCase getAllBooksByAuthorService, GetAuthorsUseCase getAuthorsService,
            UpdateAuthorUseCase updateAuthorService)
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
            var result = await getAuthorsService.GetAuthorById(authorId);
            if(result == null)
            {
                return BadRequest(new EntityNotFoundException("Author", authorId));
            }
             return Ok(result);
        }

        [HttpPost("addNewAuthor")]
        [Authorize(Policy = "LibrarianOnly")] //Доступ для RoleName = Librarian и выше
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddNewAuthor([FromBody] AuthorModel author, [FromServices] IValidator<AuthorModel> validator)
        {
            var validationResult = await validator.ValidateAsync(author);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return Ok(await addNewAuthorService.AddNewAuthor(author));
        }

        [HttpPost("updateAuthor")]
        [Authorize(Policy = "LibrarianOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAuthor(int authorId, [FromBody] UpdateAuthorRequest data, [FromServices] IValidator<UpdateAuthorRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(data);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return Ok(await updateAuthorService.UpdateAuthor(authorId, data));
        }

        [HttpDelete("deleteAuthor")]
        [Authorize(Policy = "LibrarianOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAuthor(int authorId)
        {
            var result = await deleteAuthorService.DeleteAuthor(authorId);
            if(result == 0)
            {
                return BadRequest(new EntityNotFoundException("Author", authorId));
            }
            return Ok(result);
        }

        [HttpGet("getAllBooksByAuthor")]
        [Authorize(Policy = "UserOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllBooksByAuthor(int authorId)
        {
            var result = await getAllBooksByAuthorService.GetAllBooksByAuthor(authorId);
            if (result.Count() == 0) 
            {
                return BadRequest(new EntityNotFoundException("Author", authorId));
            }
            return Ok(result);
        }
    }
}
