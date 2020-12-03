using System;
using System.Data.SQLite;
using System.Threading.Tasks;
using AppSettings;
using Domain;
using Grpc.Net.Client;
using LoggerLibrary;
using LoggerLibrary.Rabbit;
using Microsoft.Extensions.Configuration;
using Services;

namespace LogServer
{
    class ServerLogs
    {
        private static LogServiceRemote _logServiceRemote;
        static void Main(string[] args)
        {
            IConfiguration configuration = AppSettingsFactory.GetAppSettings();
            
            var channel = GrpcChannel.ForAddress(configuration["GrpcServer"]);
            _logServiceRemote = new LogServiceRemote(channel);
            
            // Rabbit connection init
            string rabbitHost = configuration["RabbitHost"];
            using var rabbitClient = new RabbitQueueHelper(
                rabbitHostname: rabbitHost,
                queueName: configuration["LogQueueName"]
            );
            rabbitClient.QueueDeclare();
            rabbitClient.ReceiveMessages(LogRemoteHandler);

            Console.WriteLine($"Listening queue: {configuration["LogQueueName"]}");
            Console.Read();
        }

        public static async Task LogRemoteHandler(string message)
        {
            await _logServiceRemote.SaveLogAsync(new Log
            {
               date = DateTime.Now,
               Message = message
            });
        }
    }
}