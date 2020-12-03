using System.Collections.Generic;
using Domain;
using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CommentList
{
    public class CommentListResponse : Response
    {
        public string Username { get; }
        public string PhotoName { get; }
        public ICollection<Comment> Comments { get; }

        public CommentListResponse(
            string username,
            string photoName,
            ICollection<Comment> comments
        ) : base(MessageId.CommentList)
        {
            PhotoName = photoName;
            Comments = comments;
            Username = username;
        }
    }
}