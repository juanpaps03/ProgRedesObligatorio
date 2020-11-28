using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CommentPhoto
{
    public class CommentPhotoRequest : Request
    {
        public string NamePhoto { get; }
        public string UserName { get; }
        public string Text { get; }

        public CommentPhotoRequest(string namePhoto, string userName, string text) : base(MessageId.CommentPhoto)
        {
            NamePhoto = namePhoto;
            UserName = userName;
            Text = text;
        }
    }
}