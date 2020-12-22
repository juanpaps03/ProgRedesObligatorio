using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;
using Domain.Responses;
using WebApi.Helpers;
using WebApi.Responses;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers
{
    [Route("users")]
    [ApiController]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<WebPaginatedResponse<User>>> GetStudentsAsync(int page = 1, int pageSize = 15)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }

            PaginatedResponse<User> usersPaginatedResponse =
                await _userService.GetUsersAsync(page, pageSize);
            if (usersPaginatedResponse == null)
            {
                return NoContent();
            }

            string route = _httpContextAccessor.HttpContext.Request.Host.Value +
                           _httpContextAccessor.HttpContext.Request.Path;
            WebPaginatedResponse<User> response =
                WebPaginationHelper<User>.GenerateWebPaginatedResponse(usersPaginatedResponse, page, pageSize, route);

            return Ok(response);
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
            if (responseUser == null)
            {
                return BadRequest("Non existing user");
            }
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