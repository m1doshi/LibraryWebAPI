using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Application.DTOs;
using WebAPI.Infrastructures.Persistence;
using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructures.Repositories
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
            return await dbContext.Users.Where(u => u.UserID == userId).Select(u => new UserModel
            {
                UserID = u.UserID,
                UserName = u.UserName,
                PasswordHash = u.PasswordHash,
                Email = u.Email
            }).FirstOrDefaultAsync();
        }

        public async Task<UserModel> GetUserByRefreshToken(string refreshToken)
        {
            return await dbContext.Users.Where(u => u.RefreshToken == refreshToken).Select(u => new UserModel
            {
                RefreshToken = u.RefreshToken,
                RefreshTokenExpireTime = u.RefreshTokenExpireTime
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateUser(UserModel updatedUser)
        {
            var user = await dbContext.Users.FindAsync(updatedUser.UserID);
            if (user == null) return false;
            user.RefreshTokenExpireTime = updatedUser.RefreshTokenExpireTime;
            user.RefreshToken = updatedUser.RefreshToken;
            return true;
        }

    }
}
