using Business_Layer.Services;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var user = await _userService.GetUserByEmailAndPasswordAsync(model.Email, model.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var tokenString = _userService.GenerateJwtToken(user);
            return Ok(new { Token = tokenString });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (await _userService.UserExistsAsync(model.Email))
            {
                return BadRequest("Email already exists");
            }

            var newUser = new User
            {
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            };

            await _userService.AddUserAsync(newUser);

            var tokenString = _userService.GenerateJwtToken(newUser);
            return Ok(new { Token = tokenString });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUserDetails(int userId, User updatedUser)
        {
            if (userId != updatedUser.UserId)
            {
                return BadRequest("User ID in the URL must match the ID in the request body.");
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = updatedUser.Email;
            user.Role = updatedUser.Role;

            await _userService.UpdateUserAsync(user);

            return Ok("User details updated successfully.");
        }

        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(user);

            return Ok(new { Message = "User deleted successfully." });
        }
    }
}
    

