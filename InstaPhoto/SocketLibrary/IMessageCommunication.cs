namespace SocketLibrary
{
    public interface IMessageCommunication
    {
        public void SendMessage(Message msg);

        public Message ReceiveMessage();
    }
}