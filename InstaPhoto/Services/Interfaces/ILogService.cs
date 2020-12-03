using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Responses;

namespace Services
{
    public interface ILogService
    {
        Task<Log> SaveLogAsync(Log log);
        
        Task<IEnumerable<Log>> GetLogsAsync();
        Task<PaginatedResponse<Log>> GetLogsAsync(int page, int pageSize);
    }
}