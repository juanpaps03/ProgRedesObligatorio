using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.UserList
{
    public class UserListRequestHandler : IContentHandler<UserListRequest>
    {
        public UserListRequestHandler()
        {
        }

        public async Task SendMessageAsync(UserListRequest msg)
        {
        }

        public async Task<UserListRequest> ReceiveMessageAsync()
        {
            return new UserListRequest();
        }
    }
}