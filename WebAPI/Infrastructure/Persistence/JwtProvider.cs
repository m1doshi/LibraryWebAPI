using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using WebAPI.Infrastructures.Interfaces;
using WebAPI.Core.DTOs;
using WebAPI.Core.Entities;

namespace WebAPI.Infrastructures.Persistence
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions options;
        public JwtProvider(IOptions<JwtOptions> options)
        {
            this.options = options.Value;
        }
        public string GenerateToken(User user)
        {
            Claim[] claims = [new("userId", user.UserID.ToString()), new("roleId",user.RoleID.ToString())];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(options.ExpiresHours)
                );
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
