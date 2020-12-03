using SocketLibrary.Constants;

namespace SocketLibrary.Messages
{
    public abstract class Message
    {
        public MessageType Type { get; }
        public MessageId Id { get; }

        protected Message(MessageType type, MessageId id)
        {
            Type = type;
            Id = id;
        }
    }
}