using System.Collections.Generic;
using Domain;
using SocketLibrary.Constants;

namespace SocketLibrary.Messages.UserList
{
    public class UserListResponse: Response
    {
        public ICollection<User> Users { get; }
        
        public UserListResponse(ICollection<User> users) : base(MessageId.UserList)
        {
            Users = users;
        }
    }
}