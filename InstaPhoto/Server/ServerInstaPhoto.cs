using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Grpc.Net.Client;
using SocketLibrary.Constants;

namespace Server
{
    class ServerInstaPhoto
    {
        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), ProtocolSpecification.Port));
            tcpListener.Start(20);
            while (true) // TODO: CHANGE FOR REAL CONDITION
            {
                var tcpClient = tcpListener.AcceptTcpClient();
                var clientHandler = new ClientHandler(
                    stream: tcpClient.GetStream(),
                    channel
                );
                clientHandler.ExecuteAsync().ContinueWith(
                    t => Console.WriteLine(t.Exception),
                    TaskContinuationOptions.OnlyOnFaulted
                );
            }

            tcpListener.Stop();
        }
    }
}