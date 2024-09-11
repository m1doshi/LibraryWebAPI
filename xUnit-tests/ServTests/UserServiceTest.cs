using Microsoft.EntityFrameworkCore;
using Xunit;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Entities;
using WebAPI.Database;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using WebAPI.Services;
using WebAPI.Infrastructure;
using Microsoft.Extensions.Options;
using System.Drawing.Text;
using WebAPI.Repositories.Interfaces;
using WebAPI.Infrastructure.Interfaces;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;


namespace xUnit_tests.ServTests
{
    public class UserServiceTest
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IUserRepository> userRepository;
        private readonly Mock<IPasswordHasher> passwordHasher;
        private readonly Mock<IJwtProvider> jwtProvider;
        private readonly UserService userService;

        public UserServiceTest()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            userRepository = new Mock<IUserRepository>();
            passwordHasher = new Mock<IPasswordHasher>();
            jwtProvider = new Mock<IJwtProvider>();
            unitOfWork.Setup(u=>u.Users).Returns(userRepository.Object);
            unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            userService = new UserService(unitOfWork.Object, passwordHasher.Object, jwtProvider.Object);
        }

        [Fact]
        public async Task Register_ShouldRegisterNewUser()
        {
            string userName = "testName";
            string email = "testEmail";
            string password = "testPassword";
            string hashedPassword = "testHashedPassword";
            passwordHasher.Setup(p => p.Generate(password)).Returns(hashedPassword);
            userRepository.Setup(r => r.AddNewUser(It.IsAny<UserModel>())).ReturnsAsync(true);

            var result = await userService.Register(userName, email, password);
            Assert.NotEqual(0, result);

            passwordHasher.Verify(p => p.Generate(password), Times.Once());
            userRepository.Verify(r => r.AddNewUser(It.IsAny<UserModel>()), Times.Once());
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once());
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenValid()
        {
            string email = "testEmail";
            string password = "testPassword";
            string hashedPassword = "testHashedPassword";
            string token = "testToken";

            var user = new UserModel
            {
                UserName = "testUser",
                Email = email,
                PasswordHash = hashedPassword
            };

            userRepository.Setup(r => r.GetUserByEmail(email)).ReturnsAsync(user);
            passwordHasher.Setup(p => p.Verify(password, hashedPassword)).Returns(true);
            jwtProvider.Setup(j => j.GenerateToken(user)).Returns(token);

            var result = await userService.Login(email, password);

            Assert.Equal(token, result);
            userRepository.Verify(r => r.GetUserByEmail(email), Times.Once());
            passwordHasher.Verify(p => p.Verify(password, hashedPassword), Times.Once());
            jwtProvider.Verify(j => j.GenerateToken(user), Times.Once());
        }

        [Fact]
        public async Task Login_ShouldThrowException_WhenPasswordIsInvalid()
        {
            string email = "testEmail";
            string password = "testPassword";
            string hashedPassword = "testHashedPassword";

            var user = new UserModel
            {
                UserName = "testUser",
                Email = email,
                PasswordHash = hashedPassword
            };

            userRepository.Setup(r => r.GetUserByEmail(email)).ReturnsAsync(user);
            passwordHasher.Setup(p => p.Verify(password, hashedPassword)).Returns(false);

            await Assert.ThrowsAsync<Exception>(() => userService.Login(email, password));

            userRepository.Verify(r => r.GetUserByEmail(email), Times.Once());
            passwordHasher.Verify(p => p.Verify(password, hashedPassword), Times.Once());
        }

    }
}
