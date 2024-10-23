using Consumer.MongoDB;
using Consumer.property;
using Microsoft.Extensions.Configuration;

namespace Consumer
{
    internal partial class Program
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

            var mongoDbConfig = new MongoDbConfig
            {
                ConnectionString = config.GetSection("MongoDb:ConnectionString").Value!.ToString(),
                DatabaseName = config.GetSection("MongoDb:DatabaseName").Value!.ToString(),
                CollectionName = config.GetSection("MongoDb:CollectionName").Value!.ToString()
            };
            var anomalyDetectionConfig = new AnomalyDetectionConfig
            {
                MemoryUsageAnomalyThresholdPercentage = double.Parse(config.GetSection("AnomalyDetectionConfig:MemoryUsageAnomalyThresholdPercentage").Value!),
                CpuUsageAnomalyThresholdPercentage = double.Parse(config.GetSection("AnomalyDetectionConfig:CpuUsageAnomalyThresholdPercentage").Value!),
                MemoryUsageThresholdPercentage = double.Parse(config.GetSection("AnomalyDetectionConfig:MemoryUsageThresholdPercentage").Value!),
                CpuUsageThresholdPercentage = double.Parse(config.GetSection("AnomalyDetectionConfig:CpuUsageThresholdPercentage").Value!)
            };

            //start consuming
            var consumer = new RabbitMqConsumer(rabbitConfig.HostName, rabbitConfig.QueueName);

            string topic = $"ServerStatistics.{serverStatsConfig.ServerIdentifier}";



            ServerStatistics preStatistics = null;

            consumer.StartListening(topic, (statistics) =>
            {

                IMongoDbService client = new MongoDbService(mongoDbConfig);


                var anomalyDetector = new AnomalyDetector(anomalyDetectionConfig);

                if (anomalyDetector.IsHighUsage(statistics))
                {
                    Console.WriteLine("High Usage Alert");
                }
                if (preStatistics != null && anomalyDetector.IsAnomalous(statistics, preStatistics))
                {
                    Console.WriteLine("Anomaly Detected");
                }

                client.InsertStatistics(statistics);
                preStatistics = statistics;

                Console.WriteLine($"Received: {statistics}");
            });

            Console.ReadKey();
        }

    }
}
