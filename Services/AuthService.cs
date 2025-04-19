using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobileProvider.Data;
using MobileProvider.Models.DTOs;
using MobileProvider.Models.Entities;
using MobileProvider.Services.Interfaces;

namespace MobileProvider.Services
{
    public class AuthService : IAuthService
    {
        private readonly MobileProviderDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(MobileProviderDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Subscribers.AnyAsync(s => s.Username == dto.Username))
                return false;

            var subscriber = new Subscriber
            {
                SubscriberNo = dto.SubscriberNo,
                Username = dto.Username,
            };
            subscriber.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            _context.Subscribers.Add(subscriber);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.Username == dto.Username);
            if (subscriber == null)
            {
                return null;
            };

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, subscriber.PasswordHash))
            {
                return null;
            }

            return GenerateJwt(subscriber);
        }

        private string GenerateJwt(Subscriber subscriber)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, subscriber.Id.ToString()),
                new Claim(ClaimTypes.Name, subscriber.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
