using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.UserList
{
    public class UserListResponseHandler : IContentHandler<UserListResponse>
    {
        private readonly INetworkCommunication _networkCommunication;

        public UserListResponseHandler(
            INetworkCommunication networkCommunication
        )
        {
            _networkCommunication = networkCommunication;
        }

        public async Task SendMessageAsync(UserListResponse msg)
        {
            // Send array of users
            await _networkCommunication.SendIntAsync(msg.Users.Count);
            foreach (var user in msg.Users)
            {
                await _networkCommunication.SendStringAsync(user.Username);
            }
        }

        public async Task<UserListResponse> ReceiveMessageAsync()
        {
            // Receive array of photos
            var users = new List<User>();
            var count = await _networkCommunication.ReceiveIntAsync();
            for (var i = 0; i < count; i++)
            {
                var userName = await _networkCommunication.ReceiveStringAsync();

                users.Add(new User
                {
                    Username = userName,
                    Password = "*****"
                });
            }
            
            return new UserListResponse(users);
        }
    }
}