using Consumer.property;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Consumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            var serverStatsConfig = new ServerStatisticsConfig
            {
                SamplingIntervalSeconds = int.Parse(config.GetSection("ServerStatisticsConfig:SamplingIntervalSeconds").Value!),
                ServerIdentifier = config.GetSection("ServerStatisticsConfig:ServerIdentifier").Value!.ToString()
            };
            var rabbitConfig = new RabbitMqConfig
            {
                HostName = config.GetSection("RabbitMq:Hostname").Value!.ToString(),
                QueueName = config.GetSection("RabbitMq:QueueName").Value!.ToString()!
            };


            //start consuming
            var consumer = new RabbitMqConsumer(rabbitConfig.HostName, rabbitConfig.QueueName);

            string topic = $"ServerStatistics.{serverStatsConfig.ServerIdentifier}";

            consumer.StartListening(topic, (statistics) =>
            {
                Console.WriteLine($"Received: {JsonSerializer.Serialize(statistics)}");
            });

            Console.ReadKey();
        }

    }
}
