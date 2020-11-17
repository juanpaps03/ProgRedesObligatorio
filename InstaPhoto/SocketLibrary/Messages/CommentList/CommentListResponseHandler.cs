using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using SocketLibrary.Interfaces;

namespace SocketLibrary.Messages.CommentList
{
    public class CommentListResponseHandler : IContentHandler<CommentListResponse>
    {
        private readonly INetworkCommunication _networkCommunication;

        public CommentListResponseHandler(
            INetworkCommunication networkCommunication
        )
        {
            _networkCommunication = networkCommunication;
        }

        public async Task SendMessageAsync(CommentListResponse msg)
        {
            // All comments are from the same photo
            await _networkCommunication.SendStringAsync(msg.Namephoto);
            
            // Send array of comments
            await _networkCommunication.SendIntAsync(msg.Comments.Count);
            foreach (var comment in msg.Comments)
            {
                await _networkCommunication.SendStringAsync(comment.Text);
            }
        }

        public async Task<CommentListResponse> ReceiveMessageAsync()
        {
            // All comments are from the same photo
            var namephoto = await _networkCommunication.ReceiveStringAsync();
            
            // Receive array of comments
            var comments = new List<Comment>();
            var count = await _networkCommunication.ReceiveIntAsync();
            for (var i = 0; i < count; i++)
            {
                var text = await _networkCommunication.ReceiveStringAsync();

                comments.Add(new Comment
                {
                    NamePhoto = namephoto,
                    Text = text
                });
            }
            
            return new CommentListResponse(namephoto, comments);
        }
    }
}