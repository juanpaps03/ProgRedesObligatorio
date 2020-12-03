using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Responses;
using Grpc.Core;
using GrpcServer;
using Services.Interfaces;

namespace Services
{
    // TODO: mover esto
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
    public class LogServiceRemote : ILogService
    {
        private readonly Logs.LogsClient _client;
        private readonly IMapper _mapper;

        public LogServiceRemote(ChannelBase channel)
        {
            _client = new Logs.LogsClient(channel);

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<long, DateTime>().ConvertUsing<LongToDateTimeConverter>();
                    cfg.CreateMap<DateTime, long >().ConvertUsing<DateTimeToLongConverter>();
                    cfg.CreateMap<LogMessage, Log>();
                    cfg.CreateMap<Log, LogMessage>();
                    cfg.CreateMap<GetPaginatedLogsReply, PaginatedResponse<Log>>();
                });
            _mapper = config.CreateMapper();
        }

        public async Task<Log> SaveLogAsync(Log log)
        {
            var logMessage = _mapper.Map<LogMessage>(log);
            var reply = await _client.SaveLogAsync(
                new SaveLogsRequest
                {
                    Log = logMessage
                }
            );
            return _mapper.Map<Log>(reply.Log);
        }

        public async Task<IEnumerable<Log>> GetLogsAsync()
        {
            var reply = await _client.GetLogsAsync(new GetLogsRequest());
            return _mapper.Map<IEnumerable<Log>>(reply.LogList);
        }

        public async Task<PaginatedResponse<Log>> GetLogsAsync(int page, int pageSize)
        {
            var reply = await _client.GetPaginatedLogsAsync(
                new GetPaginatedLogsRequest
                {
                    Page = page,
                    PageSize = pageSize
                }
            );
            return _mapper.Map<PaginatedResponse<Log>>(reply);
        }
    }
}