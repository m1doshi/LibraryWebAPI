using Microsoft.EntityFrameworkCore;
using WebAPI.Application.DTOs;
using WebAPI.Infrastructures.Persistence;
using WebAPI.Domain.Entities;
using System.Net;
using WebAPI.Domain.Exceptions;
using WebAPI.Domain.Interfaces.Repositories;

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
            var user = await dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            return user == null ? null : new UserModel
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpireTime = user.RefreshTokenExpireTime
            };
        }

        public async Task<UserModel> GetUserById(int userId)
        {
            var user = await dbContext.Users.FindAsync(userId);
            return user == null ? null : new UserModel
            {
                UserID = user.UserID,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                Email = user.Email
            };
        }

        public async Task<UserModel> GetUserByRefreshToken(string refreshToken)
        {
            var user = await dbContext.Users.Where(u => u.RefreshToken == refreshToken).FirstOrDefaultAsync();
            return user == null ? null : new UserModel
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpireTime = user.RefreshTokenExpireTime
            };
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
