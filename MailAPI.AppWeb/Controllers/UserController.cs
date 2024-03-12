using MailAPI.Data.Models;
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
        [HttpPost("RegisterUser")]
        public IActionResult CreateUser(string FirstName, string LastName, string Email, string password, int roleid)
        {
            var createUser = UserService.RegisterUser(FirstName,LastName,Email,password,roleid);
            return Ok();
        }
        [HttpGet("GetUserByID")]
        public Task<User> GetUser(int id) 
        {
            return UserService.GetUserById(id);

        }
        [HttpGet("GetRole")]
        public Task<Role> GetRole(int id) 
        {
            return UserService.GetRole(id);
        }
        [HttpPost("EditUser")]
        public Task<User> EditUser(int id, string Email,string password)
        {
            return UserService.EditUser(id, Email, password);

        }
        [HttpDelete("DeleteUser")]
        public Task<bool> DeleteUser(string Email, string password)
        {
            return UserService.DeleteUser(Email, password);

        }
        [HttpGet("LoginUser")]
        public Task<bool> LoginUser(string Email, string password)
        {
            return UserService.Login(Email, password);
        }
        [HttpDelete("Logout")]
        public Task<bool> Logout(string Email) 
        {
             return UserService.Logout(Email);
            
        }


    }
}
