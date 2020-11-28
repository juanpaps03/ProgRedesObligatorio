using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CommentList
{
    public class CommentListRequest : Request
    {
        public string Username { get; }
        public string PhotoName { get; }

        public CommentListRequest(string username, string photoName) : base(MessageId.CommentList)
        {
            Username = username;
            PhotoName = photoName;
        }
    }
}