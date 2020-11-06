namespace SocketLibrary
{
    public interface INetworkCommunication
    {
        public void SendBytes(byte[] bytes);
        public byte[] ReceiveBytes(int length);
        public void SendShort(short data);
        public short ReceiveShort();
        public void SendInt(int data);
        public int ReceiveInt();
        public void SendLong(long data);
        public long ReceiveLong();
        public void SendString(string data);
        public string ReceiveString();
    }
}