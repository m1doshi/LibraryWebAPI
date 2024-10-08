using Microsoft.EntityFrameworkCore;
using WebAPI.DataAccess;
using WebAPI.DataAccess.Repositories;
using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;

namespace xUnit_tests.InfrastructureTests.Repositories
{
    public class UserRepositoryTest
    {
        private MyDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new MyDbContext(options);
        }

        [Fact]
        public async Task AddNewUser_ShouldAddNewUser()
        {
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);

            var newUser = new UserModel()
            {
                UserName = "test1",
                Email = "test1",
                PasswordHash = "test1"
            };
            await repository.AddNewUser(newUser);
            await context.SaveChangesAsync();
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == "test1");

            Assert.NotNull(user);
            Assert.Equal("test1", user.Email);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnUser_WhenUserExists()
        {
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);
            var newUser = new User()
            {
                UserName = "test1",
                Email = "test1",
                PasswordHash = "test1"
            };
            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();

            var user = await repository.GetUserByEmail("test1");
            Assert.NotNull(user);
            Assert.Equal("test1", user.Email);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);
            var user = await repository.GetUserByEmail("test");
            Assert.Null(user);
        }

        [Fact]
        public async Task UpdateUser_ShouldUpdateUser_WhenUserExists()
        {
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);
            var newUser = new User
            {
                UserID = 1,
                UserName = "test1",
                PasswordHash = "test1",
                Email = "test1",
                RefreshToken = "qwerty",
                RefreshTokenExpireTime = DateTime.UtcNow,
                RoleID = 2
            };
            context.Users.Add(newUser);
            await context.SaveChangesAsync();
            var updatedUser = new UserModel
            {
                UserID = newUser.UserID,
                RoleID = 3,
                RefreshToken = "newQwerty",
                RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7)
            };
            await repository.UpdateUser(updatedUser);
            await context.SaveChangesAsync();
            var user = await context.Users.FindAsync(1);
            Assert.Equal("newQwerty", user.RefreshToken);
            Assert.Equal(3, user.RoleID);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);
            var updatedUser = new UserModel
            {
                UserID = 100,
                RefreshToken = "newQwerty",
                RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7)
            };
            await repository.UpdateUser(updatedUser);
            await context.SaveChangesAsync();
            var user = context.Users.Find(100);
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByRefreshToken_ShouldReturnUser_WhenUserExists()
        {
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);
            var newUser = new User
            {
                UserID = 1,
                UserName = "test1",
                PasswordHash = "test1",
                Email = "test1",
                RefreshToken = "qwerty",
                RefreshTokenExpireTime = DateTime.UtcNow
            };
            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();
            var user = await repository.GetUserByRefreshToken("qwerty");
            Assert.Equal(1, user.UserID);
            Assert.Equal("test1", user.UserName);
        }

        [Fact]
        public async Task GetUserByRefreshToken_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);
            var user = await repository.GetUserByRefreshToken("doesNotExist");
            Assert.Null(user);
        }
    }
}
