namespace SocketLibrary
{
    public interface IContentBaseHandler {}
    public interface IContentHandler<T>: IContentBaseHandler where T : Message
    {
        public void SendMessage(T msg);

        public T ReceiveMessage();
    }
}