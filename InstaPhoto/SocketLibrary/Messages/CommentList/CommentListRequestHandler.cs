using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.CommentList
{
    public class CommentListRequestHandler: IContentHandler<CommentListRequest>
    {
        private readonly INetworkCommunication _networkCommunication;

        public CommentListRequestHandler(INetworkCommunication networkCommunication)
        {
            _networkCommunication = networkCommunication;
        }

        public async Task SendMessageAsync(CommentListRequest msg)
        {
            await _networkCommunication.SendStringAsync(msg.Username);
            await _networkCommunication.SendStringAsync(msg.PhotoName);
        }

        public async Task<CommentListRequest> ReceiveMessageAsync()
        {
            var username = await _networkCommunication.ReceiveStringAsync();
            var photoName = await _networkCommunication.ReceiveStringAsync();
            
            return new CommentListRequest(username, photoName);
        }
    }
}