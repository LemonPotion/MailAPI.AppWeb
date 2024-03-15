using MailAPI.Data.Models;

namespace MailAPI.Services.IServices
{
    public interface IContactService
    {
        Task<bool> AddContactHistory(int userId, string email, string name, string description);
        Task<bool> DeleteContactHistory(int id);
        Task<bool> UpdateContactHistory(int id, string email, string name, string description);
        Task<ContactHistory> GetContactHistory(int id);
    }
}
