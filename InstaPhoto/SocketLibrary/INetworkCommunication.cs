namespace SocketLibrary
{
    public interface INetworkCommunication
    {
        public void SendBytes(byte[] bytes);
        public byte[] ReceiveBytes(int length);
        public void SendShort(short data);
        public short ReceiveShort(int lenght);
        public void SendInt(int data);
        public int ReceiveInt(int lenght);
        public void SendLong(long data);
        public long ReceiveLong(int lenght);
        public void SendString(string data);
        public string ReceiveString(int lenght);
    }
}