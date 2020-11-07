namespace SocketLibrary.Messages.Login
{
    public class LoginRequestHandler: IContentHandler<LoginRequest>
    {
        private NetworkCommunication _networkStream;

        public LoginRequestHandler(NetworkCommunication networkStream)
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