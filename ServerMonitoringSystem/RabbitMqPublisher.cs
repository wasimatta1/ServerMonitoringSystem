using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ServerMonitoringSystem
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly string _hostName;
        private readonly string _queueName;

        public RabbitMqPublisher(string hostName, string queueName)
        {
            _hostName = hostName;
            _queueName = queueName;
        }

        public void Publish(string topic, ServerStatistics statistics)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: topic,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            string message = JsonSerializer.Serialize(statistics);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: topic, basicProperties: null, body: body);
            Console.WriteLine($"[x] Sent {message}");
        }
    }
}
