using SocketLibrary.Constants;

namespace SocketLibrary.Messages.Logout
{
    public class LogoutResponse: Response
    {
        public LogoutResponse() : base(MessageId.Logout)
        {
        }
    }
}