using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LoggerLibrary.Rabbit
{
    public static class RabbitClient
    {
        // TODO: agregar esta data a un App.config 
        private const string QueueName = "log";
        private const string HostName = "localhost";
        public static string QueueDeclare(IModel channel)
        {
            channel.QueueDeclare(QueueName, false, false, false, null);
            return QueueName;
        }
        public static void ReceiveMessages(IModel channel, Action<string> handler)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handler(message);
            };
            channel.BasicConsume(QueueName, true, consumer);
        }
        
        public static void SendMessage(string message)
        {
            var connectionFactory = new ConnectionFactory { HostName = HostName };
            using IConnection connection = connectionFactory.CreateConnection();
            using IModel channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: string.Empty, routingKey: QueueName, basicProperties: null, body: body);
        }
    }
}