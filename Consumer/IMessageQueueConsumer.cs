namespace Consumer
{
    public interface IMessageQueueConsumer
    {
        void StartListening(string topic, Action<ServerStatistics> onMessageReceived);
    }
}
