using WebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;

namespace WebAPI.Infrastructure
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions options;
        public JwtProvider(IOptions<JwtOptions> options) 
        { 
            this.options = options.Value;
        }
        public string GenerateToken(UserModel user)
        {
            Claim[] claims = [new("userId", user.UserID.ToString())];

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
