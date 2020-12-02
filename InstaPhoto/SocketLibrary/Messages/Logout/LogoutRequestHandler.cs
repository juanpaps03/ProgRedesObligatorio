using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.Logout
{
    public class LogoutRequestHandler: IContentHandler<LogoutRequest>
    {
        public async Task SendMessageAsync(LogoutRequest msg)
        {
        }

        public async Task<LogoutRequest> ReceiveMessageAsync()
        {
            return new LogoutRequest();
        }
    }
}