using SocketLibrary.Constants;

namespace SocketLibrary.Messages.Logout
{
    public class LogoutRequest: Request
    {
        public LogoutRequest() : base(MessageId.Logout)
        {
        }
    }
}