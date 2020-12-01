using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("users")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<User>> SaveUserAsync(User user)
        {
            var responseUser = await _userService.SaveUserAsync(user);
            return new CreatedResult(string.Empty, responseUser);
        }
        
        [HttpPut]
        public async Task<ActionResult<User>> UpdateUserAsync(User user)
        {
            var responseUser = await _userService.UpdateUserAsync(user);
            return Ok(responseUser);
        }
        
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUserAsync(string userName)
        {
            var user = await _userService.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(user);
            return NoContent();
        }
    }
}