namespace ServerMonitoringSystem
{
    internal interface IRabbitMqPublisher
    {
        void Publish(string topic, ServerStatistics statistics);
    }
}
