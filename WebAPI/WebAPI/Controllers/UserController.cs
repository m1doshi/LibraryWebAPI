using Microsoft.AspNetCore.Mvc;
using WebAPI.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Application.UseCases.Users;
using FluentValidation;
using Application.UseCases.Users;
using WebAPI.DataAccess.Exceptions;

namespace WebAPI.API.Controllers
{
    [ApiController]
    [Route("userController")]
    public class UserController : ControllerBase
    {
        private readonly LoginUseCase loginService;
        private readonly RegisterUseCase registerService;
        private readonly UpdateTokensUseCase updateTokensService;
        private readonly UpdateUserUseCase updateUserService;
        private readonly DeleteUserUseCase deleteUserService;
        private readonly GetUsersUseCase getUsersService;


        public UserController(LoginUseCase loginService,
            RegisterUseCase registerService,
            UpdateTokensUseCase updateTokensService,
            UpdateUserUseCase updateUserService,
            DeleteUserUseCase deleteUserService,
            GetUsersUseCase getUsersService)
        {
            this.registerService = registerService;
            this.loginService = loginService;
            this.updateTokensService = updateTokensService;
            this.updateUserService = updateUserService;
            this.deleteUserService = deleteUserService;
            this.getUsersService = getUsersService;
        }
        
        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] RegisterUserRequest userRequest, [FromServices] IValidator<RegisterUserRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(userRequest);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            var result = await registerService.Register(userRequest.UserName, userRequest.Email, userRequest.Password);
            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] LoginUserRequest userRequest, [FromServices] IValidator<LoginUserRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(userRequest);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return Ok(await loginService.Login(userRequest.Email, userRequest.Password));
        }

        [HttpPost("refresh-token")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateTokens([FromBody] RefreshTokenRequest tokenRequest, [FromServices] IValidator<RefreshTokenRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(tokenRequest);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return Ok(await updateTokensService.UpdateTokens(tokenRequest));
        }

        [HttpPost("updateUser")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUser([FromBody] UserModel updatedUser, [FromServices] IValidator<UserModel> validator)
        {
            var validationResult = await validator.ValidateAsync(updatedUser);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            return Ok(await updateUserService.UpdateUser(updatedUser));
        }

        [HttpGet("getAllUsers")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllUsers()
        {
            return Ok(await getUsersService.GetAllUsers());
        }

        [HttpGet("getUserById")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserById(int userId)
        {
            var result = await getUsersService.GetUserById(userId);
            if (result == null)
            {
                return BadRequest(new EntityNotFoundException("User", userId));
            }
            return Ok(result);
        }

        [HttpGet("getUserByEmail")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserByEmail(string email)
        {
            var result = await getUsersService.GetUserByEmail(email);
            if (result == null)
            {
                return BadRequest(new EntityNotFoundException("User", email));
            }
            return Ok(result);
        }

        [HttpGet("getUserByRefreshToken")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserByRefreshToken(string token)
        {
            var result = await getUsersService.GetUserByRefreshToken(token);
            if (result == null)
            {
                return BadRequest(new EntityNotFoundException("User", token));
            }
            return Ok(result);
        }

        [HttpDelete("deleteUser")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var result = await deleteUserService.DeleteUser(userId);
            if (result == 0)
            {
                return BadRequest(new EntityNotFoundException("User", userId));
            }
            return Ok(result);
        }
    }
}
