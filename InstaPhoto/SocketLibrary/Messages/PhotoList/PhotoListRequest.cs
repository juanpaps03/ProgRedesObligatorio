using SocketLibrary.Constants;

namespace SocketLibrary.Messages.PhotoList
{
    public class PhotoListRequest : Request
    {
        public string Username { get; }
        
        public PhotoListRequest(string username) : base(MessageId.PhotoList)
        {
            Username = username;
        }
    }
}