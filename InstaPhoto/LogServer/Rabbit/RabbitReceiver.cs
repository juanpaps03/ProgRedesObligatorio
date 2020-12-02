using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogServer.Rabbit
{
    public class RabbitReceiver
    {
        public static void ListenQueue(string queueName, Action<string> handler, IModel channel)
        {
            QueueDeclare(queueName, channel);
            ReceiveMessages(queueName, channel, handler);
        }
        public static void QueueDeclare(string queueName, IModel channel)
        {
            channel.QueueDeclare(queueName, false, false, false, null);
        }
        public static void ReceiveMessages(string queueName, IModel channel, Action<string> handler)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handler(message);
            };
            channel.BasicConsume(queueName, true, consumer);
        }
    }
}