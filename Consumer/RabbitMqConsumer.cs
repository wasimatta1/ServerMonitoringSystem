using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Consumer
{
    public class RabbitMqConsumer : IMessageQueueConsumer
    {
        private readonly string _hostName;
        private readonly string _queueName;

        public RabbitMqConsumer(string hostName, string queueName)
        {
            _hostName = hostName;
            _queueName = queueName;
        }

        public void StartListening(string topic, Action<ServerStatistics> onMessageReceived)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: topic, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var statistics = JsonSerializer.Deserialize<ServerStatistics>(message);

                onMessageReceived?.Invoke(statistics);
            };

            channel.BasicConsume(queue: topic, autoAck: true, consumer: consumer);

            Console.WriteLine("Consuming");
        }
    }
}
