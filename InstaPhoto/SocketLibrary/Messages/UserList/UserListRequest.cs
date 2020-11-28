using SocketLibrary.Constants;

namespace SocketLibrary.Messages.UserList
{
    public class UserListRequest : Request
    {
        
        public UserListRequest() : base(MessageId.UserList)
        {
        }
    }
}