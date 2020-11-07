using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.Login
{
    public class LoginRequestHandler: IContentHandler<LoginRequest>
    {
        private NetworkCommunication _networkStream;

        public LoginRequestHandler(NetworkCommunication networkStream)
        {
            _networkStream = networkStream;
        }

        public Task SendMessageAsync(LoginRequest msg)
        {
            throw new System.NotImplementedException();
        }

        public Task<LoginRequest> ReceiveMessageAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}