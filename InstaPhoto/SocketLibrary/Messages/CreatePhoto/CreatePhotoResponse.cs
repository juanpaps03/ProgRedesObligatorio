using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CreatePhoto
{
    public class CreatePhotoResponse : Response
    {
        public CreatePhotoResponse() : base(MessageId.CreatePhoto)
        {
        }
    }
}