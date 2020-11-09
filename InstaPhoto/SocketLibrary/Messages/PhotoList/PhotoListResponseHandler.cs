using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.PhotoList
{
    public class PhotoListResponseHandler : IContentHandler<PhotoListResponse>
    {
        private readonly INetworkCommunication _networkCommunication;
        private readonly IFileCommunication _fileCommunication;

        public PhotoListResponseHandler(
            INetworkCommunication networkCommunication,
            IFileCommunication fileCommunication
        )
        {
            _networkCommunication = networkCommunication;
            _fileCommunication = fileCommunication;
        }

        public async Task SendMessageAsync(PhotoListResponse msg)
        {
            // All photos are from the same user
            await _networkCommunication.SendStringAsync(msg.Photos.First().Username);
            
            // Send array of photos
            await _networkCommunication.SendIntAsync(msg.Photos.Count);
            foreach (var photo in msg.Photos)
            {
                await _networkCommunication.SendStringAsync(photo.Name);
                await _fileCommunication.SendFileAsync(photo.File);
            }
        }

        public async Task<PhotoListResponse> ReceiveMessageAsync()
        {
            // All photos are from the same user
            var username = await _networkCommunication.ReceiveStringAsync();
            
            // Receive array of photos
            var photos = new List<Photo>();
            var count = await _networkCommunication.ReceiveIntAsync();
            for (var i = 0; i < count; i++)
            {
                var name = await _networkCommunication.ReceiveStringAsync();
                var file = await _fileCommunication.ReceiveFileAsync();

                photos.Add(new Photo
                {
                    Name = name,
                    File = file,
                    Username = username
                });
            }
            
            return new PhotoListResponse(photos);
        }
    }
}