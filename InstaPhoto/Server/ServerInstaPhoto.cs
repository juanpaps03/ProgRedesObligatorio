using System;
using System.Data.SQLite;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SocketLibrary.Constants;

namespace Server
{
    class ServerInstaPhoto
    {
        static void Main(string[] args)
        {
            const string connectionString =
                @"Data Source=/home/juanpaps03/Documents/ORT/ProgramacionDeredes/ProgRedesObligatorio/dbInstaPhoto.db;foreign keys=true;Version=3;";
            var connection = new SQLiteConnection(connectionString);

            var tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), ProtocolSpecification.Port));
            tcpListener.Start(20);
            while (true) // TODO: CHANGE FOR REAL CONDITION
            {
                Console.WriteLine("when is this being executed?");
                var tcpClient = tcpListener.AcceptTcpClient();
                var clientHandler = new ClientHandler(
                    stream: tcpClient.GetStream(),
                    connection
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