using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CommentList
{
    public class CommentListRequest : Request
    {
        public string Namephoto { get; }
        
        public CommentListRequest(string namephoto) : base(MessageId.CommentList)
        {
            Namephoto = namephoto;
        }
    }
}