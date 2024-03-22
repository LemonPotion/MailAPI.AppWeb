using MailAPI.Data.Models;
using MailAPI.Services;
using MailAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MailAPI.AppWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService contactService;

        public ContactController( IContactService contactService)
        {
            this.contactService = contactService;
        }

        [HttpGet("GetContact")]
        public Task<ContactHistory> GetContact(int id)
        {
           return contactService.GetContactHistory(id);
        }
        [HttpPost("AddContact")]
        public Task<bool> AddContact(int userId ,string email,string name , string description)
        {
           return contactService.AddContactHistory(userId,email, name, description);
        }
        [HttpDelete("DeleteContact")]
        public Task<bool> DeleteContact(int id)
        {
            return contactService.DeleteContactHistory(id);
        }
        [HttpPost("EditContact")]
        public Task<bool> EditContact(int id, string email, string name, string description)
        {
            return contactService.UpdateContactHistory(id,email,name,description);
        }
    }
}
