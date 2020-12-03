using SocketLibrary.Constants;

namespace SocketLibrary.Messages
{
    public abstract class Response: Message
    {
        protected Response(MessageId id) : base(MessageType.Response, id)
        {
        }
    }
}