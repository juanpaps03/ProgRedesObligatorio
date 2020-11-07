using System;
using System.Net;
using System.Net.Sockets;
using SocketLibrary;
using SocketLibrary.Messages;
using SocketLibrary.Messages.Login;

namespace Server
{
    class ServerInstaPhoto
    {
        private static TcpListener _tcpListener;

        private static Response HandlerLogin(Request request)
        {
            // TODO Hacer el login
            return new LoginResponse();
        }

        static void Main(string[] args)
        {
            _tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000));
            _tcpListener.Start(1);
            while (true)
            {
                TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                NetworkStream stream = tcpClient.GetStream();
                var protocolCommunication = new ProtocolCommunication(stream);
                protocolCommunication.HandleRequest(HandlerLogin);
                // TODO: Implement this code
                // while (true)
                // {
                //     protocolCommunication.HandleRequest(HandlerRequest);
                // }
            }
            _tcpListener.Stop();
        }
    }
}