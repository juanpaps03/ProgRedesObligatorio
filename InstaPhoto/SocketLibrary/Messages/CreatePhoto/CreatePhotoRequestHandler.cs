using System;
using System.Threading.Tasks;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.CreatePhoto
{
    public class CreatePhotoRequestHandler: IContentHandler<CreatePhotoRequest>
    {
        private readonly INetworkCommunication _networkCommunication;
        private readonly IFileCommunication _fileCommunication;

        public CreatePhotoRequestHandler(
            INetworkCommunication networkCommunication, 
            IFileCommunication fileCommunication
            )
        {
            _networkCommunication = networkCommunication;
            _fileCommunication = fileCommunication;
        }


        public async Task SendMessageAsync(CreatePhotoRequest msg)
        {
            await _networkCommunication.SendStringAsync(msg.Name);
            await _fileCommunication.SendFileAsync(msg.FilePath);
        }

        public async Task<CreatePhotoRequest> ReceiveMessageAsync()
        {
            var name = await _networkCommunication.ReceiveStringAsync();
            var filePath = await _fileCommunication.ReceiveFileAsync();
            
            return new CreatePhotoRequest(name, filePath);
        }
    }
}