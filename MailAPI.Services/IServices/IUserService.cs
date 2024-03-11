using MailAPI.Data.Models;

namespace MailAPI.Services
{
    public interface IUserService
    {
        Task RegisterUser(string FirstName, string LastName, string Email, string password);
        Task<bool> Login(string Email, string Password);
        Task<bool> DeleteUser(string Email, string Password);
        Task<User> GetUserById(int id);
        Task<User> EditUser(int id , string Email, string Password);
        Task<bool> Logout(string Email);
    }
}
