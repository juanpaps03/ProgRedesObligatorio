using SocketLibrary.Constants;

namespace SocketLibrary.Messages.Login
{
    public class LoginResponse: Response
    {
        public LoginResponse() : base(MessageId.Login)
        {
        }
    }
}