using GestRehema.Data;
using GestRehema.Entities;
using GestRehema.Extensions;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.Linq;

namespace GestRehema.Services
{
    public interface IUserService
    {
        User? Login(string username, string password);
        User? Register(User user);
        int UpdatePassword(string username, string oldPassword, string newPassword);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();
        }

        public User? Register(User user)
        {
            user.Password = user.Password.Hash();
            user.CreatedAt = DateTime.UtcNow;
            _dbContext.Users.Add(user);
            var userRegResult = _dbContext.SaveChanges();
            if (userRegResult > 0)
                return _dbContext.Users.First(x => x.CreatedAt == user.CreatedAt);
            return null;
        }

        public User? Login(string username, string password)
        {
            var user = _dbContext.Users
                .Include(x => x.Employee)
                .ThenInclude(x => x.Entreprise)
                .SingleOrDefault(x => x.Username == username);

            if (user != null && user.Password.Verify(password))
                return user;
            throw new Exception("Le nom d'utilisateur ou mot de passe est incorrect");
        }

        public int UpdatePassword(string username, string oldPassword, string newPassword)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.Username == username);
            if (user != null && user.Password.Verify(oldPassword))
            {
                user.Password = newPassword;
                _dbContext.Users.Update(user);
            }

            return _dbContext.SaveChanges();
        }
    }
}
