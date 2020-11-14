using System;
using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.CreatePhoto
{
    public class CommentPhotoRequestHandler: IContentHandler<CommentPhotoRequest>
    {
        private readonly INetworkCommunication _networkCommunication;
        private readonly IFileCommunication _fileCommunication;

        public CommentPhotoRequestHandler(
            INetworkCommunication networkCommunication
        )
        {
            _networkCommunication = networkCommunication;
        }

        public async Task SendMessageAsync(CommentPhotoRequest msg)
        {
            await _networkCommunication.SendStringAsync(msg.Text);
        }

        public async Task<CommentPhotoRequest> ReceiveMessageAsync()
        {
            var namePhoto = await _networkCommunication.ReceiveStringAsync();
            var userName = await _networkCommunication.ReceiveStringAsync();
            var text = await _networkCommunication.ReceiveStringAsync();
            
            return new CommentPhotoRequest(namePhoto, userName, text);
        }
    }
}