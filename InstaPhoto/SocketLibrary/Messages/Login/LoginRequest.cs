using SocketLibrary.Constants;

namespace SocketLibrary.Messages.Login
{
    public class LoginRequest: Request
    {
        public LoginRequest() : base(MessageId.Login)
        {
        }
    }
}