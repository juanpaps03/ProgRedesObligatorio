using System.Collections.Generic;
using Domain;
using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CommentList
{
    public class CommentListResponse: Response
    {
        public string Namephoto { get; }
        public ICollection<Comment> Comments { get; }
        
        public CommentListResponse(string namephoto, ICollection<Comment> comments) : base(MessageId.CommentList)
        {
            Comments = comments;
            Namephoto = namephoto;
        }
    }
}