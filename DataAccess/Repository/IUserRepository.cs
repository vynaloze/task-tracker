using System.Collections.Generic;
using DataAccess.Model;

namespace DataAccess.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser(int id);
        void InsertUser(User user);
        void DeleteUser(int id);
        void UpdateUser(User oldUser, User newUser);
        void Save();
    }
}