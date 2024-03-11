using MailAPI.Data.Models;
using MailAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MailAPI.AppWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IEmailService emailSender;

        public MessageController(IEmailService emailSender)
        {
            this.emailSender = emailSender;
        }
        [HttpPost("SendEmail")]
        public Task<bool> send_email(int SenderId,string email , string subject , string body)
        {
            return emailSender.SendEmail(SenderId,email , subject , body);
        }
        [HttpGet("GetEmailByID")]
        public Task<Message> GetMessage(int id)
        {
            return emailSender.GetMessageById(id);
        }
        [HttpPost("EditMessage")]
        public Task<Message> EditMessage(int id , string body , string subject)
        {
            return emailSender.EditMessage(id,body,subject);
        }
        [HttpDelete("DeleteMessage")]
        public Task<bool> DeleteMessage(int id) 
        {
            return emailSender.DeleteMessage(id);
        }
        [HttpGet("GetMessageHistoryList")]
        public Task<List<MessageHistory>> GetMessageHistory()
        {
            return emailSender.GetMessageHistory();
        }
    }
}
