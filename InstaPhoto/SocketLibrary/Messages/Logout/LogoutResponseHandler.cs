using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.Logout
{
    public class LogoutResponseHandler: IContentHandler<LogoutResponse>
    {
        public async Task SendMessageAsync(LogoutResponse msg)
        {
        }

        public async Task<LogoutResponse> ReceiveMessageAsync()
        {
            return new LogoutResponse();
        }
    }
}