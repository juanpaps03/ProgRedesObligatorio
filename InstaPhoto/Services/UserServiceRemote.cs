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
                    cfg.CreateMap<GetPaginatedUsersReply, PaginatedResponse<User>>();
                });
            _mapper = config.CreateMapper();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var reply = await _client.GetUsersAsync(new GetUsersRequest());
            return _mapper.Map<IEnumerable<User>>(reply.UserList);
        }

        public async Task<PaginatedResponse<User>> GetUsersAsync(int page, int pageSize)
        {
            var reply = await _client.GetPaginatedUsersAsync(
                new GetPaginatedUsersRequest
                {
                    Page = page,
                    PageSize = pageSize
                }
            );
            return _mapper.Map<PaginatedResponse<User>>(reply);
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

        public async Task<User> UpdateUserAsync(User user)
        {
            var userMessage = _mapper.Map<UserMessage>(user);
            var reply = await _client.UpdateUserAsync(
                new UpdateUserRequest {User = userMessage}
            );
            return _mapper.Map<User>(reply.User);
        }

        public async Task DeleteUserAsync(User user)
        {
            var userMessage = _mapper.Map<UserMessage>(user);
            await _client.DeleteUserAsync(
                new DeleteUserRequest {User = userMessage}
            );
        }
    }
}