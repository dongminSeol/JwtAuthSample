using JwtAuthenticationSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthenticationSample.Services
{
    public interface IUserData
    {
        User GetByUser(string userName);
        User GetByUser(string userName, string passwordHash);
    }

    public class UserData : IUserData
    {
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

    }
}
