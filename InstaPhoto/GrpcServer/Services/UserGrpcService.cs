using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace GrpcServer.Services
{
    public class UserGrpcService : Users.UsersBase
    {
        private readonly ILogger<UserGrpcService> _logger;

        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserGrpcService(ILogger<UserGrpcService> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<UserMessage, User>();
                    cfg.CreateMap<User, UserMessage>();
                });
            _mapper = config.CreateMapper();
        }

        public override async Task<GetUsersReply> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            var userList = await _userService.GetUsersAsync();
            return new GetUsersReply
            {
                UserList = {_mapper.Map<IEnumerable<UserMessage>>(userList)}
            };
        }

        public override async Task<GetUserByUserNameReply> GetUserByUserName(GetUserByUserNameRequest request,
            ServerCallContext context)
        {
            var user = await _userService.GetUserByUserNameAsync(request.Username);
            return new GetUserByUserNameReply
            {
                User = _mapper.Map<UserMessage>(user)
            };
        }

        public override async Task<SaveUserReply> SaveUser(SaveUserRequest request, ServerCallContext context)
        {
            var requestUser = _mapper.Map<User>(request.User);
            var responseUser = await _userService.SaveUserAsync(requestUser);
            return new SaveUserReply
            {
                User = _mapper.Map<UserMessage>(responseUser)
            };
        }
    }
}