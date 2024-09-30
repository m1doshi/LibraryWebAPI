using Azure;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebAPI.API.Controllers;
using WebAPI.Application.DTOs;
using WebAPI.Application.UseCases.Users;
using WebAPI.Application.Validators;
using WebAPI.Domain.Interfaces.UnitOfWork;
using WebAPI.Infrastructure.Interfaces;
using WebAPI.Infrastructures.Interfaces;

namespace xUnit_tests.ApiTests.Controllers
{
    public class UserControllerTest
    {
        private readonly UserController controller;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IPasswordHasher> passwordHasher;
        private readonly Mock<IJwtProvider> jwtProvider;
        private readonly Mock<IRefreshProvider> refreshProvider;
        private readonly Mock<IValidator<LoginUserRequest>> mockLoginUserRequestValidator;
        private readonly Mock<IValidator<RegisterUserRequest>> mockRegisterUserRequestValidator;
        private readonly Mock<IValidator<UserModel>> mockUserModelValidator;
        private readonly Mock<IValidator<RefreshTokenRequest>> mockRefreshTokenRequestValidator;
        private readonly Mock<LoginUseCase> mockLoginService;
        private readonly Mock<RegisterUseCase> mockRegisterService;
        private readonly Mock<UpdateTokensUseCase> mockUpdateTokensService;
        private readonly Mock<UpdateUserUseCase> mockUpdateUserService;
        
        public UserControllerTest()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            passwordHasher = new Mock<IPasswordHasher>();
            jwtProvider = new Mock<IJwtProvider>();
            refreshProvider = new Mock<IRefreshProvider>();
            mockLoginUserRequestValidator = new Mock<IValidator<LoginUserRequest>>();
            mockRegisterUserRequestValidator = new Mock<IValidator<RegisterUserRequest>>();
            mockRefreshTokenRequestValidator = new Mock<IValidator<RefreshTokenRequest>>();
            mockUserModelValidator = new Mock<IValidator<UserModel>>();
            mockLoginService = new Mock<LoginUseCase>(unitOfWork.Object, passwordHasher.Object, jwtProvider.Object, refreshProvider.Object);
            mockRegisterService = new Mock<RegisterUseCase>(unitOfWork.Object, passwordHasher.Object);
            mockUpdateTokensService = new Mock<UpdateTokensUseCase>(unitOfWork.Object, jwtProvider.Object, refreshProvider.Object);
            mockUpdateUserService = new Mock<UpdateUserUseCase>(unitOfWork.Object);
            controller = new UserController(mockLoginService.Object, mockRegisterService.Object, mockUpdateTokensService.Object,
                mockUpdateUserService.Object);
        }

        [Fact]
        public async Task Login_LoginUserRequestIsValid_ReturnOkResult()
        {
            var responce = new AuthenticationResponce
            {
                Token = "testJWTtoken",
                RefreshToken = "testRefreshToken"
            };
            var userRequest = new LoginUserRequest { Email = "test@email.com", Password = "testPassword" };
            mockLoginUserRequestValidator.Setup(v => v.ValidateAsync(userRequest, default)).ReturnsAsync(new ValidationResult());
            mockLoginService.Setup(s => s.Login(userRequest.Email, userRequest.Password)).ReturnsAsync(responce);
            var result = await controller.Login(userRequest, mockLoginUserRequestValidator.Object);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(responce, okResult.Value);

        }

        [Fact]
        public async Task Login_LoginUserRequestIsInvalid_ReturdBadRequest()
        {
            var userRequest = new LoginUserRequest { Email = "", Password = "testPassword" };
            mockLoginUserRequestValidator.Setup(v => v.ValidateAsync(userRequest, default))
                .ThrowsAsync(new ValidationException("Email is required"));
            var result = await controller.Login(userRequest, mockLoginUserRequestValidator.Object);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email is requied", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_RegisterUserRequestIsValid_ReturnOkResult()
        {
            var userRequest = new RegisterUserRequest
            {
                UserName = "testUserName",
                Password = "testPassword",
                Email = "test@email.com"
            };
            mockRegisterUserRequestValidator.Setup(v=>v.ValidateAsync(userRequest, default)).ReturnsAsync(new ValidationResult());
            mockRegisterService.Setup(s => s.Register(userRequest.UserName, userRequest.Email, userRequest.Password)).ReturnsAsync(1);
            var result = await controller.Register(userRequest, mockRegisterUserRequestValidator.Object);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<int>(okResult.Value);
            Assert.Equal(1, okResult.Value);
        }

        [Fact]
        public async Task Register_RegisterUserRequestIsInvalid_ReturnBadRequest()
        {
            var userRequest = new RegisterUserRequest
            {
                UserName = "t",
                Password = "testPassword",
                Email = "test@email.com"
            };
            mockRegisterUserRequestValidator.Setup(v => v.ValidateAsync(userRequest, default))
                .ThrowsAsync(new ValidationException("Username length can't be more than 20 and less than 3."));
            var result = await controller.Register(userRequest, mockRegisterUserRequestValidator.Object);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username length can't be more than 20 and less than 3.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateTokens_UpdateTokensRequestIsValid_ReturnOkResult()
        {
            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = "12345678123456781234567812345678"
            };
            var responce = new AuthenticationResponce
            {
                Token = "newJwtToken",
                RefreshToken = "newRefreshTokenNewRefreshToken12"
            };
            mockRefreshTokenRequestValidator.Setup(v => v.ValidateAsync(refreshTokenRequest, default)).ReturnsAsync(new ValidationResult());
            mockUpdateTokensService.Setup(s => s.UpdateTokens(refreshTokenRequest)).ReturnsAsync(responce);
            var result = await controller.UpdateTokens(refreshTokenRequest, mockRefreshTokenRequestValidator.Object);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(responce, okResult.Value);
        }

        [Fact]
        public async Task UpdateTokens_UpdateTokensRequestIsInvalid_ReturnBadRequest()
        {
            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = "12"
            };
            mockRefreshTokenRequestValidator.Setup(v => v.ValidateAsync(refreshTokenRequest, default))
                .ThrowsAsync(new ValidationException("Refresh token length must be equal to 32."));
            var result = await controller.UpdateTokens(refreshTokenRequest, mockRefreshTokenRequestValidator.Object);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Refresh token length must be equal to 32.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUser_UserModelIsValid_ReturnOkResult() {
            var user = new UserModel
            {
                UserName = "testName",
                Email = "test@email.com",
                RoleID = 2
            };
            mockUserModelValidator.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new ValidationResult());
            mockUpdateUserService.Setup(s => s.UpdateUser(user)).ReturnsAsync(1);
            var result = await controller.UpdateUser(user, mockUserModelValidator.Object);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<int>(okResult.Value);
            Assert.Equal(1, okResult.Value);
        }

        [Fact]
        public async Task UpdateUser_UserModelIsInvalid_ReturnBadRequest()
        {
            var user = new UserModel
            {
                UserName = "",
                Email = "test@email.com",
                RoleID = 2
            };
            mockUserModelValidator.Setup(v => v.ValidateAsync(user, default)).
                ThrowsAsync(new ValidationException("Username is required."));
            var result = await controller.UpdateUser(user, mockUserModelValidator.Object);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username is required.", badRequestResult.Value);
        }

    }
}
