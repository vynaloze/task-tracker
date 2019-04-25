using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Model;
using Service.Users.Dto;

namespace Service.Users
{
    public interface IUserService
    {
        Task<User>Authenticate(string username,string password);
        Task<IEnumerable<User>>GetAll();
        Task<User>GetUser(int id);
        Task<User>RegisterUser(User user);
        Task<bool>SaveUserData(int id, User user);
        Task<bool> DeleteUser(int id);
        Task<bool> RequestResetPassword(string email);
        Task<bool> PerformResetPassword(ResetPasswordDto dto);
    }
}