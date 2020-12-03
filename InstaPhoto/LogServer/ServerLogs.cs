using System;
using AppSettings;
using LoggerLibrary;
using LoggerLibrary.Rabbit;
using Microsoft.Extensions.Configuration;

namespace LogServer
{
    class ServerLogs
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = AppSettingsFactory.GetAppSettings();

            using var rabbitClient = new RabbitQueueHelper(
                rabbitHostname: configuration["RabbitHost"],
                queueName: configuration["LogQueueName"]
            );
            rabbitClient.QueueDeclare();
            rabbitClient.ReceiveMessages(LogFileHandler);

            Console.WriteLine($"Listening queue: {configuration["LogQueueName"]}");
            Console.Read();
        }

        private static void LogFileHandler(string message)
        {
            Console.WriteLine($"Saving log message: {message}");
            ILogger logger = new FileLogger();
            logger.SaveLog(message);
        }
    }
}