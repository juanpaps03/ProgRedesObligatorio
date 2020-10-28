using System;
using System.Net.Sockets;
using System.Text;

namespace SocketLibrary
{
    public class NetworkCommunication: INetworkCommunication
    {
        private Socket _socket;

        public NetworkCommunication(Socket vSocket)
        {
            _socket = vSocket;
        }
        
        private void Send(byte[] data)
        {
            var offset = 0;
            while (offset < data.Length)
            {
                var sent = _socket.Send(
                    data,
                    offset,
                    data.Length - offset,
                    SocketFlags.None);
                if (sent == 0)
                    throw new Exception("Connection lost");
                offset += sent;
            }
        }

        public byte[] Receive(int length)
        {
            int offset = 0;
            var data = new byte[length];
            while (offset < length)
            {
                var received = _socket.Receive(
                    data,
                    offset,
                    length - offset,
                    SocketFlags.None);
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

        public short ReceiveShort(int lenght)
        {
            byte[] data = ReceiveBytes(lenght);
            return BitConverter.ToInt16(data, 0);
        }

        public void SendInt(int data)
        {
            SendLong(data);
        }

        public int ReceiveInt(int lenght)
        {
            
            byte[] data = ReceiveBytes(lenght);
            return BitConverter.ToInt32(data, 0);
        }

        public void SendLong(long data)
        {
            byte[] dataBytes = BitConverter.GetBytes(data);
            SendBytes(dataBytes);
        }

        public long ReceiveLong(int lenght)
        {
            byte[] data = ReceiveBytes(lenght);
            return BitConverter.ToInt64(data, 0);
        }

        public void SendString(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            SendBytes(dataBytes);
        }

        public string ReceiveString(int lenght)
        {
            byte[] dataBytes = ReceiveBytes(lenght);
            return Encoding.UTF8.GetString(dataBytes);
        }
    }
}