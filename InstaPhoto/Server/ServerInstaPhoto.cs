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
        private static bool _exit;

        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var tcpListener = new TcpListener(
                new IPEndPoint(
                    IPAddress.Parse("127.0.0.1"),
                    ProtocolSpecification.Port
                )
            );
            tcpListener.Start(20);
            
            AcceptClientsAsync(tcpListener, channel);

            while (!_exit)
            {
                Console.Clear();
                Console.Write("Write \"exit\" to close the server: ");
                var command = Console.ReadLine();

                _exit = command != null && command == "exit";
            }

            tcpListener.Stop();
        }

        private static async Task AcceptClientsAsync(TcpListener tcpListener, GrpcChannel channel)
        {
            while (!_exit)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                var clientHandler = new ClientHandler(
                    stream: tcpClient.GetStream(),
                    channel
                );
                clientHandler.ExecuteAsync().ContinueWith(
                    t => Console.WriteLine(t.Exception),
                    TaskContinuationOptions.OnlyOnFaulted
                );
            }
        }
    }
}