using Azure.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebAPI.Application.DTOs;
using WebAPI.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Application.UseCases.Users;
using FluentValidation;
using WebAPI.Domain.Entities;
using System.ComponentModel.DataAnnotations;

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

        public UserController(LoginUseCase loginService,
            RegisterUseCase registerService,
            UpdateTokensUseCase updateTokensService,
            UpdateUserUseCase updateUserService)
        {
            this.registerService = registerService;
            this.loginService = loginService;
            this.updateTokensService = updateTokensService;
            this.updateUserService = updateUserService;
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



    }
}
