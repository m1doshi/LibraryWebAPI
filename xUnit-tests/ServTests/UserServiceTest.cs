using Moq;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.Interfaces.UnitOfWork;
using WebAPI.Application.UseCases.Users;
using WebAPI.Application.DTOs;
using WebAPI.Infrastructures.Interfaces;


namespace xUnit_tests.ServTests
{
    public class UserServiceTest
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IUserRepository> userRepository;
        private readonly Mock<IPasswordHasher> passwordHasher;
        private readonly Mock<IJwtProvider> jwtProvider;
        private readonly RegisterUseCase registerUseCase;
        private readonly LoginUseCase loginUseCase;

        public UserServiceTest()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            userRepository = new Mock<IUserRepository>();
            passwordHasher = new Mock<IPasswordHasher>();
            jwtProvider = new Mock<IJwtProvider>();
            unitOfWork.Setup(u=>u.Users).Returns(userRepository.Object);
            unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            registerUseCase = new RegisterUseCase(unitOfWork.Object, passwordHasher.Object, jwtProvider.Object);
            loginUseCase = new LoginUseCase(unitOfWork.Object, passwordHasher.Object, jwtProvider.Object);
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

            var result = await registerUseCase.Register(userName, email, password);
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

            var result = await loginUseCase.Login(email, password);

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

            await Assert.ThrowsAsync<Exception>(() => loginUseCase.Login(email, password));

            userRepository.Verify(r => r.GetUserByEmail(email), Times.Once());
            passwordHasher.Verify(p => p.Verify(password, hashedPassword), Times.Once());
        }

    }
}
