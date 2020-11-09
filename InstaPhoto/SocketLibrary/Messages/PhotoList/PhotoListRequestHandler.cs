using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.PhotoList
{
    public class PhotoListRequestHandler: IContentHandler<PhotoListRequest>
    {
        private readonly INetworkCommunication _networkCommunication;

        public PhotoListRequestHandler(INetworkCommunication networkCommunication)
        {
            _networkCommunication = networkCommunication;
        }

        public async Task SendMessageAsync(PhotoListRequest msg)
        {
            await _networkCommunication.SendStringAsync(msg.Username);
        }

        public async Task<PhotoListRequest> ReceiveMessageAsync()
        {
            var username = await _networkCommunication.ReceiveStringAsync();
            
            return new PhotoListRequest(username);
        }
    }
}