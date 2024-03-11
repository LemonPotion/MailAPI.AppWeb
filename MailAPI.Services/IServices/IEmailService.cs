using MailAPI.Data.Models;

namespace MailAPI.Services
{
    public interface IEmailService
    {
        Task<List<MessageHistory>> GetMessageHistory();
        Task<Message> GetMessageById(int Id);
        Task<Message> EditMessage(int Id,string body,string subject);
        Task<bool> DeleteMessage(int Id);
        Task<bool> SendEmail(int userId,string email, string subject, string message);
    }
}
