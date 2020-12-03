using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Dtos;

namespace Repositories
{
    public interface ILogRepository
    {
        Task<LogDto> SaveLogAsync(LogDto logDto);
        Task<IEnumerable<LogDto>> GetLogsAsync();
        Task<IEnumerable<LogDto>> GetLogsAsync(int page, int pageSize);
        Task<int> GetTotalLogs();
    }
}