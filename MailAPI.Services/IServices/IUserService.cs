using MailAPI.Data.Models;

namespace MailAPI.Services
{
    public interface IUserService
    {
        Task CreateUser(string FirstName, string LastName, string Email, string password);
    }
}
