using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Helpers;
using Domain.Responses;
using GrpcServer;
using Repositories;
using Repositories.Dtos;
using Repositories.Interfaces;

namespace Services
{
    public class LogServiceLocal : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogServiceLocal(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        public async Task<Log> SaveLogAsync(Log log)
        {
            try
            {
                LogDto logDto = MapLogDomainToDto(log);
                var responseLogDto = await _logRepository.SaveLogAsync(logDto);
                return MapLogDtoToDomain(responseLogDto);
            }
            catch
            {
                throw new DatabaseSaveError();
            }
        }

        public async Task<IEnumerable<Log>> GetLogsAsync()
        {
            IEnumerable<LogDto> logsDto = await _logRepository.GetLogsAsync();
            return logsDto.Select(logDto => MapLogDtoToDomain(logDto)).ToList();
        }

        public async Task<PaginatedResponse<Log>> GetLogsAsync(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return null;
            }

            int totalLogs = await _logRepository.GetTotalLogs();
            if (totalLogs > 0)
            {
                IEnumerable<LogDto> repoLogs = await _logRepository.GetLogsAsync(page, pageSize);
                if (repoLogs == null || !repoLogs.Any())
                {
                    return null;
                }

                var logs = new List<Log>();
                foreach (var logDto in repoLogs)
                {
                    logs.Add(MapLogDtoToDomain(logDto));
                }

                return PaginationHelper<Log>.GeneratePaginatedResponse(pageSize, totalLogs, logs);
            }

            return PaginationHelper<Log>.GeneratePaginatedResponse(pageSize, totalLogs, new List<Log>());
        }

        private Log MapLogDtoToDomain(LogDto logDto)
        {
            DateTime date = DateTimeOffset.FromUnixTimeSeconds(logDto.date).DateTime;
            return new Log()
            {
                date = date,
                Message = logDto.Message
            };
        }

        private LogDto MapLogDomainToDto(Log log)
        {
            long unixDateTime = ((DateTimeOffset) log.date).ToUnixTimeSeconds();
            return new LogDto()
            {
                date = unixDateTime,
                Message = log.Message
            };
        }
    }
}