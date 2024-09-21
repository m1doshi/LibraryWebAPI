using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTOs;
using WebAPI.Application.Interfaces.Services.Users;

namespace WebAPI.API.Controllers
{
    [ApiController]
    [Route("userController")]
    public class UserController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly IRegisterService registerService;

        public UserController(ILoginService loginService, IRegisterService registerService)
        {
            this.registerService = registerService;
            this.loginService = loginService;
        }

        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register(RegisterUserRequest userRequest)
        {
            var result = await registerService.Register(userRequest.UserName, userRequest.Email, userRequest.Password);
            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login(LoginUserRequest userRequest)
        {
            return Ok(await loginService.Login(userRequest.Email, userRequest.Password));
        }



    }
}
