using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.CreateUser
{
    public class CreateUserRequestHandler: IContentHandler<CreateUserRequest>
    {
        private INetworkCommunication _networkStream;

        public CreateUserRequestHandler(INetworkCommunication networkStream)
        {
            _networkStream = networkStream;
        }

        public async Task SendMessageAsync(CreateUserRequest login)
        {
            await _networkStream.SendStringAsync(login.UserName);
            await _networkStream.SendStringAsync(login.Password);
        }

        public async Task<CreateUserRequest> ReceiveMessageAsync()
        {
            string userName = await _networkStream.ReceiveStringAsync();
            string password = await _networkStream.ReceiveStringAsync();
            return new CreateUserRequest(userName, password);
        }
    }
}