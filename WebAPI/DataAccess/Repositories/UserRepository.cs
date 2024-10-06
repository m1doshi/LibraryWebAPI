using WebAPI.Core.Interfaces.Repositories;
using WebAPI.Core.DTOs;
using WebAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.DataAccess.Repositories
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
            return await dbContext.Users.Where(user=>user.Email == email).Select(user=>new UserModel
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpireTime = user.RefreshTokenExpireTime
            }).FirstOrDefaultAsync();
        }

        public async Task<UserModel> GetUserById(int userId)
        {
            return await dbContext.Users.Where(user => user.UserID == userId).Select(user => new UserModel
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpireTime = user.RefreshTokenExpireTime
            }).FirstOrDefaultAsync();
        }

        public async Task<UserModel> GetUserByRefreshToken(string refreshToken)
        {
            return await dbContext.Users.Where(user => user.RefreshToken == refreshToken).Select(user => new UserModel
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpireTime = user.RefreshTokenExpireTime
            }).FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateUser(UserModel updatedUser)
        {
            var user = await dbContext.Users.FindAsync(updatedUser.UserID);
            user.UserName = updatedUser.UserName;
            user.RoleID = updatedUser.RoleID;
            user.RefreshTokenExpireTime = updatedUser.RefreshTokenExpireTime;
            user.RefreshToken = updatedUser.RefreshToken;
            return user.UserName != null;
        }

    }
}
