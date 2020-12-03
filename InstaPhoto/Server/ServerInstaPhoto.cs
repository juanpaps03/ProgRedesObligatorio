using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using AppSettings;
using Grpc.Net.Client;
using LoggerLibrary;
using LoggerLibrary.Rabbit;
using Microsoft.Extensions.Configuration;
using SocketLibrary.Constants;

namespace Server
{
    class ServerInstaPhoto
    {
        private static bool _exit;

        private static readonly object ClientsLock = new object();

        private static readonly Dictionary<Guid, (DateTime, ClientHandler)> Clients =
            new Dictionary<Guid, (DateTime, ClientHandler)>();

        private static ILogger _logger;

        static async Task Main(string[] args)
        {
            IConfiguration configuration = AppSettingsFactory.GetAppSettings();

            // Init logger
            using var rabbitQueueHelper = new RabbitQueueHelper(
                rabbitHostname: configuration["RabbitHost"],
                queueName: configuration["LogQueueName"]
            );
            _logger = new RemoteLogger(rabbitQueueHelper);

            // Init Server
            using var channel = GrpcChannel.ForAddress(configuration["GrpcServer"]);
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
                        foreach (var (id, connectionTime, name) in userList)
                        {
                            Console.WriteLine($"\t{connectionTime} - {name} ({id.ToString()})");
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
                    channel,
                    _logger
                );
                await AddClientAsync(clientHandler);

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
                        await RemoveClientAsync(clientHandler.Id);
                    }
                });
            }
        }

        private static async Task AddClientAsync(ClientHandler clientHandler)
        {
            lock (ClientsLock)
            {
                while (Clients.ContainsKey(clientHandler.Id))
                    clientHandler.Id = Guid.NewGuid();
                Clients[clientHandler.Id] = (DateTime.Now, clientHandler);
            }
        }

        private async static Task RemoveClientAsync(Guid id)
        {
            lock (ClientsLock)
            {
                Clients.Remove(id);
            }
        }

        private async static Task<List<(Guid, DateTime, string)>> GetClientNamesAsync()
        {
            var clientNames = new List<(Guid, DateTime, string)>();
            lock (ClientsLock)
            {
                foreach (var (id, (connectionTime, clientHandler)) in Clients)
                {
                    clientNames.Add(
                        (id, connectionTime, clientHandler.GetClientName())
                    );
                }
            }

            return clientNames;
        }
    }
}