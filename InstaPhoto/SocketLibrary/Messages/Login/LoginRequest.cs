using System.Reflection.Metadata;
using SocketLibrary.Constants;

namespace SocketLibrary.Messages.Login
{
    public class LoginRequest: Request
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public LoginRequest(string userName, string password) : base(MessageId.Login)
        {
            UserName = userName;
            Password = password;
        }
    }
}