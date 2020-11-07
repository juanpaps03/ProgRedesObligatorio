using System.Threading.Tasks;
using SocketLibrary.Interfaces;

#pragma warning disable 1998

namespace SocketLibrary.Messages.CreatePhoto
{
    public class CreatePhotoResponseHandler : IContentHandler<CreatePhotoResponse>
    {
        public async Task SendMessageAsync(CreatePhotoResponse msg)
        {
        }

        public async Task<CreatePhotoResponse> ReceiveMessageAsync()
        {
            return new CreatePhotoResponse();
        }
    }
}