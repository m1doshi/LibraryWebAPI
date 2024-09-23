using Azure.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Users;
using WebAPI.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.API.Controllers
{
    [ApiController]
    [Route("userController")]
    public class UserController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly IRegisterService registerService;
        private readonly IUpdateTokensService updateTokensService;
        private readonly IUpdateUserService updateUserService;

        public UserController(ILoginService loginService, 
            IRegisterService registerService, 
            IUpdateTokensService updateTokensService, 
            IUpdateUserService updateUserService)
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
        public async Task<ActionResult> Register(RegisterUserRequest userRequest)
        {
            if (!TryValidateModel(userRequest))
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                throw new ValidationException("Validation failed", validationErrors);
            }
            var result = await registerService.Register(userRequest.UserName, userRequest.Email, userRequest.Password);
            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login(LoginUserRequest userRequest)
        {
            if (!TryValidateModel(userRequest))
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                throw new ValidationException("Validation failed", validationErrors);
            }
            return Ok(await loginService.Login(userRequest.Email, userRequest.Password));
        }

        [HttpPost("refresh-token")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateTokens(RefreshTokenRequest tokenRequest)
        {
            return Ok(await updateTokensService.UpdateTokens(tokenRequest));
        }

        [HttpPost("updateUser")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUser(UserModel updatedUser)
        {
            return Ok(await updateUserService.UpdateUser(updatedUser));
        }



    }
}
