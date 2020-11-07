using System.Threading.Tasks;

namespace SocketLibrary
{
    public interface INetworkCommunication
    {
        public Task SendBytesAsync(byte[] bytes);
        public Task<byte[]> ReceiveBytesAsync(int length);
        public Task SendShortAsync(short data);
        public Task<short> ReceiveShortAsync();
        public Task SendIntAsync(int data);
        public Task<int> ReceiveIntAsync();
        public Task SendLongAsync(long data);
        public Task<long> ReceiveLongAsync();
        public Task SendStringAsync(string data);
        public Task<string> ReceiveStringAsync();
    }
}