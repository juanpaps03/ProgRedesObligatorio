using System.Reflection.Metadata;
using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CreateUser
{
    public class CreateUserRequest: Request
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public CreateUserRequest(string userName, string password) : base(MessageId.CreateUser)
        {
            UserName = userName;
            Password = password;
        }
    }
}