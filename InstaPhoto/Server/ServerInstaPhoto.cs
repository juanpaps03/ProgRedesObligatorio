using System;
using System.Collections.Generic;
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

        private static object _clientsLock = new object();
        private static Dictionary<Guid, ClientHandler> _clients = new Dictionary<Guid, ClientHandler>();

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

            Console.Clear();
            while (!_exit)
            {
                Console.Write("> ");
                var command = Console.ReadLine();

                switch (command)
                {
                    case "help":
                        Console.WriteLine("\thelp - Show system help");
                        Console.WriteLine("\texit - Stops the server");
                        Console.WriteLine("\tshow users - Displays the list of connected users");
                        break;
                    case "show users":
                        var userList = await GetClientNamesAsync();
                        foreach (var (id, name) in userList)
                        {
                            Console.WriteLine($"\t{id.ToString()} - {name}");
                        }

                        if (userList.Count == 0)
                        {
                            Console.WriteLine("\tNo users connected");
                        }
                        break;
                    case "exit":
                        _exit = true;
                        break;
                    default:
                        Console.WriteLine(
                            $"\tError: Command \"{command}\" not recognized. " +
                            "Type \"help\" to see the list of all commands."
                        );
                        break;
                }
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
                var id = await AddClientAsync(clientHandler);

                Task.Run(async () =>
                {
                    try
                    {
                        await clientHandler.ExecuteAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        await RemoveClientAsync(id);
                    }
                });
            }
        }

        private static Task<Guid> AddClientAsync(ClientHandler clientHandler)
        {
            var id = Guid.NewGuid();
            lock (_clientsLock)
            {
                while (_clients.ContainsKey(id))
                    id = Guid.NewGuid();
                _clients[id] = clientHandler;
            }

            return Task.FromResult(id);
        }

        private static Task RemoveClientAsync(Guid id)
        {
            lock (_clientsLock)
            {
                _clients.Remove(id);
            }

            return Task.FromResult(0);
        }

        private static Task<Dictionary<Guid, string>> GetClientNamesAsync()
        {
            var clientNames = new Dictionary<Guid, string>();
            lock (_clientsLock)
            {
                foreach (var (id, clientHandler) in _clients)
                {
                    clientNames[id] = clientHandler.GetClientName();
                }
            }
            
            return Task.FromResult(clientNames);
        }
    }
}