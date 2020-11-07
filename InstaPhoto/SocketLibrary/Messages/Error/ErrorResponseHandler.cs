using System.Threading.Tasks;
using SocketLibrary.Constants;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.Error
{
    public class ErrorResponseHandler: IContentHandler<ErrorResponse>
    {
        private readonly INetworkCommunication _networkCommunication;

        public ErrorResponseHandler(INetworkCommunication networkCommunication)
        {
            _networkCommunication = networkCommunication;
        }

        public async Task SendMessageAsync(ErrorResponse msg)
        {
            await _networkCommunication.SendShortAsync((short) msg.ErrorId);
            await _networkCommunication.SendStringAsync(msg.Message);
        }

        public async Task<ErrorResponse> ReceiveMessageAsync()
        {
            var errorId = await _networkCommunication.ReceiveShortAsync();
            var message = await _networkCommunication.ReceiveStringAsync();
            return new ErrorResponse((ErrorId) errorId, message);
        }
    }
}