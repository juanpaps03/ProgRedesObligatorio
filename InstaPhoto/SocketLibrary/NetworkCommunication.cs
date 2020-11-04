using System;
using System.Net.Sockets;
using System.Text;

namespace SocketLibrary
{
    public class NetworkCommunication: INetworkCommunication
    {
        private NetworkStream _networkStream;

        public NetworkCommunication(NetworkStream networkStream)
        {
            _networkStream = networkStream;
        }
        
        private void Send(byte[] data)
        {
            _networkStream.Write(
                data,
                0,
                data.Length);
            
        }

        public byte[] Receive(int length)
        {
            int offset = 0;
            var data = new byte[length];
            while (offset < length)
            {
                var received = _networkStream.Read(
                    data,
                    offset,
                    length - offset);
                if (received == 0)
                    throw new Exception("Connection lost");
                offset += received;
            }
            return data;
        }

        public void SendBytes(byte[] data)
        {
            Send(data);
        }

        public byte[] ReceiveBytes(int lenght)
        {
            return Receive(lenght);
        }

        public void SendShort(short data)
        {
            SendLong(data);
        }

        public short ReceiveShort()
        {
            byte[] data = ReceiveBytes(2);
            return BitConverter.ToInt16(data, 0);
        }

        public void SendInt(int data)
        {
            SendLong(data);
        }

        public int ReceiveInt()
        {
            
            byte[] data = ReceiveBytes(4);
            return BitConverter.ToInt32(data, 0);
        }

        public void SendLong(long data)
        {
            byte[] dataBytes = BitConverter.GetBytes(data);
            SendBytes(dataBytes);
        }

        public long ReceiveLong()
        {
            byte[] data = ReceiveBytes(8);
            return BitConverter.ToInt64(data, 0);
        }

        public void SendString(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            SendInt(dataBytes.Length);
            SendBytes(dataBytes);
        }

        public string ReceiveString()
        {
            int lenght = ReceiveInt();
            byte[] dataBytes = ReceiveBytes(lenght);
            return Encoding.UTF8.GetString(dataBytes);
        }
    }
}