using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.UserList
{
    public class UserListRequestHandler: IContentHandler<UserListRequest>
    {
        private readonly INetworkCommunication _networkCommunication;

        public UserListRequestHandler(INetworkCommunication networkCommunication)
        {
            _networkCommunication = networkCommunication;
        }

        public async Task SendMessageAsync(UserListRequest msg)
        {
            //await _networkCommunication.SendStringAsync();
        }

        public async Task<UserListRequest> ReceiveMessageAsync()
        {
            var users = await _networkCommunication.ReceiveStringAsync();
            
            return new UserListRequest();
        }
    }
}