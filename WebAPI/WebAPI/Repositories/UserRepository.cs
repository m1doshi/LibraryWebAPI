using WebAPI.Entities;
using WebAPI.Models;
using WebAPI.Database;
using Microsoft.EntityFrameworkCore;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext dbContext;

        public UserRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddNewUser(UserModel user)
        {
            User userEntity = new();
            userEntity.UserName = user.UserName;
            userEntity.PasswordHash = user.PasswordHash;
            userEntity.Email = user.Email;
            await dbContext.Users.AddAsync(userEntity);
            return true;
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await dbContext.Users.Where(u => u.Email == email).Select(u => new UserModel
            {
                UserID = u.UserID,
                UserName = u.UserName,
                PasswordHash = u.PasswordHash,
                Email = email
            }).FirstOrDefaultAsync();
        }

        public async Task<UserModel> GetUserById(int userId)
        {
            return await dbContext.Users.Where(u=>u.UserID == userId).Select(u => new UserModel 
            { 
                UserID = u.UserID,
                UserName = u.UserName,
                PasswordHash = u.PasswordHash,
                Email = u.Email
            }).FirstOrDefaultAsync();
        }
    }
}
