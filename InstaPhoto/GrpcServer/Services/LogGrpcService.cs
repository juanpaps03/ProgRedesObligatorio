using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Responses;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Services;
using Services.Interfaces;

namespace GrpcServer.Services
{
    public class LongToDateTimeConverter: ITypeConverter<long, DateTime>{
        public DateTime Convert(long source, DateTime destination, ResolutionContext context) {
            return DateTimeOffset.FromUnixTimeSeconds(source).DateTime; 
        }
    }
    public class DateTimeToLongConverter: ITypeConverter<DateTime, long>{
        public Int64 Convert(DateTime source, long destination, ResolutionContext context) {
            return ((DateTimeOffset) source).ToUnixTimeSeconds();; 
        }
    }
    public class LogGrpcService : Logs.LogsBase
    {
        private readonly ILogger<LogGrpcService> _logger;

        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public LogGrpcService(ILogger<LogGrpcService> logger, ILogService logService)
        {
            _logger = logger;
            _logService = logService;

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<long, DateTime>().ConvertUsing<LongToDateTimeConverter>();
                    cfg.CreateMap<DateTime, long >().ConvertUsing<DateTimeToLongConverter>();
                    cfg.CreateMap<LogMessage, Log>();
                    cfg.CreateMap<Log, LogMessage>();
                    cfg.CreateMap<PaginatedResponse<Log>, GetPaginatedLogsReply>();
                });
            _mapper = config.CreateMapper();
        }

        public override async Task<GetLogsReply> GetLogs(GetLogsRequest request, ServerCallContext context)
        {
            var userList = await _logService.GetLogsAsync();
            return new GetLogsReply()
            {
                LogList = {_mapper.Map<IEnumerable<LogMessage>>(userList)}
            };
        }

        public override async Task<GetPaginatedLogsReply> GetPaginatedLogs(
            GetPaginatedLogsRequest request, 
            ServerCallContext context
        )
        {
            var response = await _logService.GetLogsAsync(request.Page, request.PageSize);
            return _mapper.Map<GetPaginatedLogsReply>(response);
        }
        public override async Task<SaveLogsReply> SaveLog(SaveLogsRequest request, ServerCallContext context)
        {
            var requestLog = _mapper.Map<Log>(request.Log);
            var responseLog = await _logService.SaveLogAsync(requestLog);
            return new SaveLogsReply()
            {
                Log = _mapper.Map<LogMessage>(responseLog)
            };
        }
    }
}