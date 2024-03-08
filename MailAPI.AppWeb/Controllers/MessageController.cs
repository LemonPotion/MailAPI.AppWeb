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
        public IActionResult send_email(string email , string subject , string body)
        {
            emailSender.SendEmailAsync(email,subject,body);
            return Ok();
        }
        [HttpGet("SendEmailDb")]
        public IActionResult send_email_db(string email, string subject, string body)
        {
            emailSender.SendEmailDb(email,subject,body);
            return Ok();
        }
        [HttpGet("GetMessageHistoryList")]
        public IActionResult GetMessageHistory()
        {
            emailSender.GetMessageHistory();
            return Ok();
        }
    }
}
