namespace SocketLibrary
{
    public interface IContentHandler
    {
        public void SendMessage(Message msg);

        public Message ReceiveMessage();
    }
}