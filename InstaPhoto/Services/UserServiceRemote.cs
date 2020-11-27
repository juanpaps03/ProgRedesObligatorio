using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;
using Services.Interfaces;

namespace Services
{
    public class UserServiceRemote : IUserService
    {
        private readonly Users.UsersClient _client;
        private readonly IMapper _mapper;

        public UserServiceRemote(ChannelBase channel)
        {
            _client = new Users.UsersClient(channel);

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<UserMessage, User>();
                    cfg.CreateMap<User, UserMessage>();
                });
            _mapper = config.CreateMapper();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var reply = await _client.GetUsersAsync(new GetUsersRequest());
            return _mapper.Map<IEnumerable<User>>(reply.UserList);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var reply = await _client.GetUserByUserNameAsync(
                new GetUserByUserNameRequest {Username = userName}
            );
            return _mapper.Map<User>(reply.User);
        }

        public async Task<User> SaveUserAsync(User user)
        {
            var userMessage = _mapper.Map<UserMessage>(user);
            var reply = await _client.SaveUserAsync(
                new SaveUserRequest {User = userMessage}
            );
            return _mapper.Map<User>(reply.User);
        }
    }
}