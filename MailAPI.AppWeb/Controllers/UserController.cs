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
        [HttpGet("LoginUser")]
        public IActionResult LoginUser(string  Email, string password) 
        {
            UserService.Login(Email, password);
            return Ok();
        }
        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser(string Email, string password)
        {
            UserService.DeleteUser(Email, password);
            return Ok();
        }
        [HttpDelete("Logout")]
        public IActionResult Logout(string Email) 
        {
            UserService.Exit(Email);
            return Ok();
        }


    }
}
