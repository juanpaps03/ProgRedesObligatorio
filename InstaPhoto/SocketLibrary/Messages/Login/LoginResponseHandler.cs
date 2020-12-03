using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.Login
{
    public class LoginResponseHandler: IContentHandler<LoginResponse>
    {
        private INetworkCommunication _networkStream;

        public LoginResponseHandler(INetworkCommunication networkStream)
        {
            _networkStream = networkStream;
        }

        public async Task SendMessageAsync(LoginResponse loginResponse)
        {
        }

        public async Task<LoginResponse> ReceiveMessageAsync()
        {
            return new LoginResponse();
        }
    }
}