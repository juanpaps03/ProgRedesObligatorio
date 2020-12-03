using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;
using Domain.Responses;
using WebApi.Helpers;
using WebApi.Responses;
using Microsoft.AspNetCore.Http;
using Services;

namespace WebApi.Controllers
{
    [Route("logs")]
    [ApiController]

    public class LogController : Controller
    {
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogController(ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<WebPaginatedResponse<Log>>> GetLogsAsync(int page = 1, int pageSize = 15)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }

            PaginatedResponse<Log> logsPaginatedResponse =
                await _logService.GetLogsAsync(page, pageSize);
            if (logsPaginatedResponse == null)
            {
                return NoContent();
            }

            string route = _httpContextAccessor.HttpContext.Request.Host.Value +
                           _httpContextAccessor.HttpContext.Request.Path;
            WebPaginatedResponse<Log> response =
                WebPaginationHelper<Log>.GenerateWebPaginatedResponse(logsPaginatedResponse, page, pageSize, route);

            return Ok(response);
        }
    }
}