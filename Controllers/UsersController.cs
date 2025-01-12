using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Data;
using Microsoft.AspNetCore.Authorization;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetUsers()
        {
            return Ok(UserData.Users);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult CreateUser(User user)
        {
            user.Id = UserData.Users.Count + 1;
            UserData.Users.Add(user);
            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateUser(int id, User user)
        {
            var existingUser = UserData.Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null) return NotFound();

            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Role = user.Role;
            return Ok(existingUser);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteUser(int id)
        {
            var user = UserData.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            UserData.Users.Remove(user);
            return Ok();
        }

        [HttpGet("profile")]
        [Authorize(Policy = "UserOnly")]
        public IActionResult GetProfile()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var user = UserData.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }

}
