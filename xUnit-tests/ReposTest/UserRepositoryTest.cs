using Microsoft.EntityFrameworkCore;
using WebAPI.Application.DTOs;
using WebAPI.Infrastructures.Repositories;
using WebAPI.Domain.Entities;
using WebAPI.Infrastructures.Persistence;

namespace xUnit_tests.ReposTest
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
        public async Task GetUserByEmail_ShouldReturnNull_WhenUserDontExist()
        {
            var context = GetInMemoryDbContext();
            var repository = new UserRepository(context);
            var user = await repository.GetUserByEmail("test");
            Assert.Null(user);
        }

    }
}
