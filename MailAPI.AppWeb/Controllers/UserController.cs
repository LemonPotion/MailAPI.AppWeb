using MailAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace MailAPI.AppWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService UserService;

        public UserController(IUserService UserService)
        {
            this.UserService = UserService;
        }
        [HttpPost("CreateUser")]
        public IActionResult CreateUser(string FirstName, string LastName, string Email, string password)
        {
            UserService.CreateUser(FirstName,LastName,Email,password);
            return Ok();
        }

    }
}
