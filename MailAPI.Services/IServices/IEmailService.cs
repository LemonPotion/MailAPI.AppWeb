namespace MailAPI.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string Email , string Subject , string message);
        Task<bool> SendEmailDb(string email, string subject, string message);
        Task GetMessageHistory();
    }
}
