using MailAPI.Data;
using MailAPI.Data.Models;

namespace MailAPI.Services
{
    public interface IUserService
    {
        Task RegisterUser(string FirstName, string LastName, string Email, string password, int roleid);
        Task<bool> Login(string Email, string Password);
        Task<bool> DeleteUser(string Email, string Password);
        Task<User> GetUserById(int id);
        Task<Role> GetRole(int id);
        Task<User> EditUser(int id , string Email, string Password);
        Task<bool> Logout(string Email);
    }
}
