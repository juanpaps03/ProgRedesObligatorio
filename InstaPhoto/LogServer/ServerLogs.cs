using System;
using LogServer.Logger;
using LogServer.Rabbit;
using RabbitMQ.Client;

namespace LogServer
{
    class ServerLogs
    {
        private const string QueueName = "testQueue";

        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            using IConnection connection = connectionFactory.CreateConnection();
            using IModel channel = connection.CreateModel();

            RabbitReceiver.QueueDeclare(QueueName, channel);
            RabbitReceiver.ReceiveMessages(QueueName, channel, LogFileHandler);

            Console.WriteLine($"Listening queue: {QueueName}");
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