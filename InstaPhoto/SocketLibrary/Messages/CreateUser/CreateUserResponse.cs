using SocketLibrary.Constants;

namespace SocketLibrary.Messages.CreateUser
{
    public class CreateUserResponse: Response
    {
        public CreateUserResponse() : base(MessageId.CreateUser)
        {
        }
    }
}