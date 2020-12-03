using System;
using LoggerLibrary;
using LoggerLibrary.Rabbit;
using RabbitMQ.Client;

namespace LogServer
{
    class ServerLogs
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            using IConnection connection = connectionFactory.CreateConnection();
            using IModel channel = connection.CreateModel();

            string queueName = RabbitClient.QueueDeclare(channel);
            RabbitClient.ReceiveMessages(channel, LogFileHandler);

            Console.WriteLine($"Listening queue: {queueName}");
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