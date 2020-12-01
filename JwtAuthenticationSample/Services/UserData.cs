using JwtAuthenticationSample.JwtHelpers;
using JwtAuthenticationSample.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace JwtAuthenticationSample.Services
{
    public interface IUserData
    {
        User GetByUser(string userName);
        User GetByUser(string userName, string passwordHash);
        string GenerateToken(string userName, string passWordHash, int expireHours = 1);
    }

    public class UserData : IUserData
    {
        private readonly AppSettings _appSettings;

        public UserData(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        private List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                UserName = "Dongmin",
                PasswordHash = PasswordEncoder.Encode("Dongmin"),
                Email = "Dongmin@gmail.com",
                BirthDate = "1989-11-27"
            },
            new User
            {
                Id = 2,
                UserName = "Admin",
                PasswordHash = PasswordEncoder.Encode("Admin"),
                Email = "Admin@gmail.com",
                BirthDate = "1990-11-27"
            },
            new User
            {
                Id = 3,
                UserName = "Yeokchonmaster",
                PasswordHash = PasswordEncoder.Encode("Yeokchonmaster"),
                Email = "Yeokchonmaster@gmail.com",
                BirthDate = "1991-11-27"
            }
        };

        public User GetByUser(string userName)
        {
            return _users.FirstOrDefault(t => t.UserName == userName);
        }
        public User GetByUser(string userName, string passwordHash)
        {
            return _users.FirstOrDefault(t => t.UserName == userName && t.PasswordHash == passwordHash);
        }

        public string GenerateToken(string userName, string passWordHash, int expireHours = 1)
        {
            var symmetricKey = Convert.FromBase64String(_appSettings.Secret.ToString());
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = now.AddHours(Convert.ToInt32(expireHours)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature),

                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Hash, passWordHash)
                })
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;

        }
    }
}
