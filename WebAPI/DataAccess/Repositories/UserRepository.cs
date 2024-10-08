using WebAPI.Core.Interfaces.Repositories;
using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace WebAPI.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext dbContext;

        public UserRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddNewUser(User user)
        {
            var result = await dbContext.Users.AddAsync(user);
            return result != null;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await dbContext.Users.ToListAsync();
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await dbContext.Users.Where(user => user.Email == email).SingleOrDefaultAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await dbContext.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            return await dbContext.Users.Where(user => user.RefreshToken == refreshToken).SingleOrDefaultAsync();
        }
        public async Task<bool> DeleteUser(int userId)
        {
            var user = await dbContext.Users.FindAsync(userId);
            if (user == null) return false;
            dbContext.Users.Remove(user);
            return true;
        }
        public async Task<bool> UpdateUser(User user)
        {
            var result = dbContext.Users.Update(user);
            return result != null;
        }

    }
}
