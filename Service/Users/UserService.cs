using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Model;
using DataAccess.Repository;
using Service.Users.Dto;
using Service.Users.Util;

namespace Service.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await Task.Run(() =>
                _userRepository.GetUsers().SingleOrDefault(u => u.Email == email && u.Password == password));
            if (user == null)
                return null;
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.Run(() => _userRepository.GetUsers());
        }

        public async Task<User> GetUser(int id)
        {
            return await Task.Run(() => _userRepository.GetUser(id));
        }

        public async Task<User> RegisterUser(User user)
        {
            return await Task.Run(() =>
            {
                var existsDuplicate = _userRepository.GetUsers().Any(u => u.Email == user.Email);
                if (existsDuplicate)
                {
                    return null;
                }

                _userRepository.InsertUser(user);
                return user;
            });
        }

        public async Task<bool> SaveUserData(int id, User user)
        {
            return await Task.Run(() =>
            {
                var oldUser = _userRepository.GetUser(id);
                if (oldUser == null)
                {
                    return false;
                }

                _userRepository.UpdateUser(oldUser, user);
                return true;
            });
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await Task.Run(() =>
            {
                var oldUser = _userRepository.GetUser(id);
                if (oldUser == null)
                {
                    return false;
                }

                _userRepository.DeleteUser(id);
                return true;
            });
        }

        public async Task<bool> RequestResetPassword(string email)
        {
            return await Task.Run(() =>
            {
                var user = _userRepository.GetUsers().FirstOrDefault(u => u.Email.Equals(email));
                if (user == null)
                {
                    return false;
                }

                var token = TokenUtil.GenerateToken();
                var newUser = user.Clone();
                newUser.ResetPasswordToken = token;
                _userRepository.UpdateUser(user, newUser);

                Mailer.SendResetRequest(user.Email, token);

                return true;
            });
        }

        public async Task<bool> PerformResetPassword(ResetPasswordDto dto)
        {
            return await Task.Run(() =>
            {
                var user = _userRepository.GetUsers().FirstOrDefault(u => u.Email.Equals(dto.Email));
                if (user == null)
                {
                    return false;
                }

                if (TokenUtil.IsValid(dto.Token) && user.ResetPasswordToken.Equals(dto.Token))
                {
                    var newUser = user.Clone();
                    newUser.Password = dto.Password;
                    newUser.ResetPasswordToken = null;
                    _userRepository.UpdateUser(user, newUser);
                    return true;
                }

                return false;
            });
        }
    }
}