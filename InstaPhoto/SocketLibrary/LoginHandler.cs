using System.Net.Sockets;

namespace SocketLibrary
{
    public class LoginHandler: IContentHandler<LoginRequest>
    {
        private NetworkCommunication _networkStream;

        public LoginHandler(NetworkCommunication networkStream)
        {
            _networkStream = networkStream;
        }

        public void SendMessage(LoginRequest msg)
        {
            throw new System.NotImplementedException();
        }

        public LoginRequest ReceiveMessage()
        {
            throw new System.NotImplementedException();
        }
    }
}