using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces.UnitOfWork;

namespace Application.UseCases.Users
{
    public class GetUsersUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public GetUsersUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var users = await unitOfWork.Users.GetAllUsers();
            return users.Select(u => new UserModel
            {
                UserID = u.UserID,
                UserName = u.UserName,
                PasswordHash = u.PasswordHash,
                Email = u.Email,
                RefreshToken = u.RefreshToken,
                RefreshTokenExpireTime = u.RefreshTokenExpireTime,
                RoleID = u.RoleID
            });
        }
        public async Task<UserModel> GetUserById(int id)
        {
            var entity = await unitOfWork.Users.GetUserById(id);
            if (entity == null) return null;
            return new UserModel
            {
                UserID = entity.UserID,
                UserName = entity.UserName,
                PasswordHash = entity.PasswordHash,
                Email = entity.Email,
                RefreshToken = entity.RefreshToken,
                RefreshTokenExpireTime = entity.RefreshTokenExpireTime,
                RoleID = entity.RoleID
            };
        }
        public async Task<UserModel> GetUserByEmail(string email)
        {
            var entity = await unitOfWork.Users.GetUserByEmail(email);
            if (entity == null) return null;
            return new UserModel
            {
                UserID = entity.UserID,
                UserName = entity.UserName,
                PasswordHash = entity.PasswordHash,
                Email = entity.Email,
                RefreshToken = entity.RefreshToken,
                RefreshTokenExpireTime = entity.RefreshTokenExpireTime,
                RoleID = entity.RoleID
            };
        }
        public async Task<UserModel> GetUserByRefreshToken(string token)
        {
            var entity = await unitOfWork.Users.GetUserByRefreshToken(token);
            if (entity == null) return null;
            return new UserModel
            {
                UserID = entity.UserID,
                UserName = entity.UserName,
                PasswordHash = entity.PasswordHash,
                Email = entity.Email,
                RefreshToken = entity.RefreshToken,
                RefreshTokenExpireTime = entity.RefreshTokenExpireTime,
                RoleID = entity.RoleID
            };
        }
    }
}
