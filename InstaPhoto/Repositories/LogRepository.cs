using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Repositories.Dtos;

namespace Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly IDbConnection _dbConnection;

        public LogRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        
        public async Task<LogDto> SaveLogAsync(LogDto logDto)
        {
            _dbConnection.Open();
            await _dbConnection.InsertAsync(logDto);
            var responseUserDto = await _dbConnection.GetAsync<LogDto>(logDto.Id);
            _dbConnection.Close();
            return responseUserDto;
        }

        public async Task<IEnumerable<LogDto>> GetLogsAsync()
        {
            _dbConnection.Open();
            IEnumerable<LogDto> logs = await _dbConnection.GetAllAsync<LogDto>();
            _dbConnection.Close();
            return logs;
        }

        public async Task<IEnumerable<LogDto>> GetLogsAsync(int page, int pageSize)
        {
            _dbConnection.Open();
            try
            {
                string countSql = "SElECT COUNT(*) FROM Log";
                int totalLogs = await _dbConnection.ExecuteScalarAsync<int>(countSql);
                int offset = (page - 1) * pageSize;
                if (offset > totalLogs)
                {
                    return null;
                }

                string sql = "SELECT * FROM Log LIMIT @PageSize OFFSET @Offset";
                return await _dbConnection.QueryAsync<LogDto>(sql, new {PageSize = pageSize, Offset = offset});
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        public async Task<int> GetTotalLogs()
        {
            _dbConnection.Open();
            string sql = "SElECT COUNT(*) FROM Log";
            int totalLogs = await _dbConnection.ExecuteScalarAsync<int>(sql);
            _dbConnection.Close();
            return totalLogs;
        }
    }
}