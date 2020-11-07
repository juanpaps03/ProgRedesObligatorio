using SocketLibrary.Constants;
using SocketLibrary.Messages;

namespace SocketLibrary
{
    public abstract class Request: Message
    {
        protected Request(MessageId id) : base(MessageType.Request, id)
        {
        }
    }
}