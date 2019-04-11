using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.Find(id);
        }

        public void InsertUser(User user)
        {
            _context.Users.Add(user);
            Save();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            Save();
        }

        public void UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}