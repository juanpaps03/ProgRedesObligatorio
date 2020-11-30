using System.Threading.Tasks;
using SocketLibrary.Interfaces;

#pragma warning disable 1998

namespace SocketLibrary.Messages.CommentPhoto
{
    public class CommentPhotoResponseHandler : IContentHandler<CommentPhotoResponse>
    {
        private readonly INetworkCommunication _networkCommunication;
        
        public CommentPhotoResponseHandler(
            INetworkCommunication networkCommunication
        )
        {
            _networkCommunication = networkCommunication;
        }
        
        public async Task SendMessageAsync(CommentPhotoResponse msg)
        {
        }

        public async Task<CommentPhotoResponse> ReceiveMessageAsync()
        {
            return new CommentPhotoResponse();
        }
    }
}