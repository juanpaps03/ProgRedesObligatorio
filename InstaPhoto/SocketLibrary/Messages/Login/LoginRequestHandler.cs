using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.Login
{
    public class LoginRequestHandler: IContentHandler<LoginRequest>
    {
        private INetworkCommunication _networkStream;

        public LoginRequestHandler(INetworkCommunication networkStream)
        {
            _networkStream = networkStream;
        }

        public async Task SendMessageAsync(LoginRequest login)
        {
            await _networkStream.SendStringAsync(login.UserName);
            await _networkStream.SendStringAsync(login.Password);
        }

        public async Task<LoginRequest> ReceiveMessageAsync()
        {
            string userName = await _networkStream.ReceiveStringAsync();
            string password = await _networkStream.ReceiveStringAsync();
            return new LoginRequest(userName, password);
        }
    }
}