namespace Produser
{
    internal interface IRabbitMqPublisher
    {
        void Publish(string topic, ServerStatistics statistics);
    }
}
