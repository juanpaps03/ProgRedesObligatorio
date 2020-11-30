using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CommentPhoto
{
    public class CommentPhotoResponse : Response
    {
        public CommentPhotoResponse() : base(MessageId.CommentPhoto)
        {
        }
    }
}