using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace ServerMonitoringSystem
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serverStatsConfig = new ServerStatisticsConfig
            {
                SamplingIntervalSeconds = int.Parse(config.GetSection("ServerStatisticsConfig:SamplingIntervalSeconds").Value!),
                ServerIdentifier = config.GetSection("ServerStatisticsConfig:ServerIdentifier").Value!.ToString()
            };

            Console.WriteLine($"Sampling Interval: {serverStatsConfig.SamplingIntervalSeconds}, Server Identifier: {serverStatsConfig.ServerIdentifier}");

            while (true)
            {
                var state = CollectServerStatistics();

                Console.WriteLine($"Memory Usage: {state.MemoryUsage} MB");
                Console.WriteLine($"Available Memory: {state.AvailableMemory} MB");
                Console.WriteLine($"CPU Usage: {state.CpuUsage} %");
                Console.WriteLine($"Timestamp: {state.Timestamp}");

                IRabbitMqPublisher Publisher = new RabbitMqPublisher("localhost", "ServerStatistics");
                string topic = $"ServerStatistics.{serverStatsConfig.ServerIdentifier}";
                Publisher.Publish(topic, state);
                Console.WriteLine($"Published to {topic}");

                await Task.Delay(serverStatsConfig.SamplingIntervalSeconds * 1000);
            }
        }


        public static ServerStatistics CollectServerStatistics()
        {
            return new ServerStatistics
            {
                MemoryUsage = GC.GetTotalMemory(false) / 1024.0 / 1024.0,
                AvailableMemory = GetAvailableMemory(),
                CpuUsage = GetCpuUsage(),
                Timestamp = DateTime.Now
            };
        }
        private static double GetCpuUsage()
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            Thread.Sleep(1000);
            return cpuCounter.NextValue();
        }
        private static double GetAvailableMemory()
        {
            var memCounter = new PerformanceCounter("Memory", "Available MBytes");
            return memCounter.NextValue();
        }

    }
}
