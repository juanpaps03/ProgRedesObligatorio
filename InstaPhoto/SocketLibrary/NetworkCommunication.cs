using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketLibrary
{
    public class NetworkCommunication : INetworkCommunication
    {
        private readonly NetworkStream _networkStream;

        public NetworkCommunication(NetworkStream networkStream)
        {
            _networkStream = networkStream;
        }

        public async Task SendBytesAsync(byte[] data)
        {
            await _networkStream.WriteAsync(
                data,
                0,
                data.Length
            );
        }

        public async Task<byte[]> ReceiveBytesAsync(int length)
        {
            int offset = 0;
            var data = new byte[length];
            while (offset < length)
            {
                var received = await _networkStream.ReadAsync(
                    data,
                    offset,
                    length - offset
                );
                if (received == 0)
                    throw new Exception("Connection lost");
                offset += received;
            }

            return data;
        }

        public async Task SendShortAsync(short data)
        {
            byte[] dataBytes = BitConverter.GetBytes(data);
            await SendBytesAsync(dataBytes);
        }

        public async Task<short> ReceiveShortAsync()
        {
            byte[] data = await ReceiveBytesAsync(2);
            return BitConverter.ToInt16(data, 0);
        }

        public async Task SendIntAsync(int data)
        {
            byte[] dataBytes = BitConverter.GetBytes(data);
            await SendBytesAsync(dataBytes);
        }

        public async Task<int> ReceiveIntAsync()
        {
            byte[] data = await ReceiveBytesAsync(4);
            return BitConverter.ToInt32(data, 0);
        }

        public async Task SendBoolAsync(bool data)
        {
            byte[] dataBytes = BitConverter.GetBytes(data);
            await SendBytesAsync(dataBytes);
        }

        public async Task<bool> ReceiveBoolAsync()
        {
            byte[] data = await ReceiveBytesAsync(1);
            return BitConverter.ToBoolean(data, 0);
        }

        public async Task SendLongAsync(long data)
        {
            byte[] dataBytes = BitConverter.GetBytes(data);
            await SendBytesAsync(dataBytes);
        }

        public async Task<long> ReceiveLongAsync()
        {
            byte[] data = await ReceiveBytesAsync(8);
            return BitConverter.ToInt64(data, 0);
        }

        public async Task SendStringAsync(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            await SendIntAsync(dataBytes.Length);
            await SendBytesAsync(dataBytes);
        }

        public async Task<string> ReceiveStringAsync()
        {
            int lenght = await ReceiveIntAsync();
            byte[] dataBytes = await ReceiveBytesAsync(lenght);
            return Encoding.UTF8.GetString(dataBytes);
        }
    }
}