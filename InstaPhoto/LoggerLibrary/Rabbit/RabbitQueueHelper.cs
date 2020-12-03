using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LoggerLibrary.Rabbit
{
    public class RabbitQueueHelper: IDisposable
    {
        private readonly string _queueName;

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitQueueHelper(string rabbitHostname, string queueName)
        {
            _queueName = queueName;
            
            var connectionFactory = new ConnectionFactory { HostName = rabbitHostname };
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        
        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        public void QueueDeclare()
        {
            _channel.QueueDeclare(_queueName, false, false, false, null);
        }
        public void ReceiveMessages(Action<string> handler)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handler(message);
            };
            _channel.BasicConsume(_queueName, true, consumer);
        }
        
        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: string.Empty, routingKey: _queueName, basicProperties: null, body: body);
        }
    }
}