using MailAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MailAPI.AppWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IEmailSender emailSender;

        public MessageController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }
        [HttpPost]
        public IActionResult send_email(string email , string subject , string body)
        {
            emailSender.SendEmailAsync(email,subject,body);
            return Ok();
        }
    }
}
