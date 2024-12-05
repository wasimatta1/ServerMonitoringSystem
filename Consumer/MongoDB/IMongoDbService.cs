namespace Consumer.MongoDB
{
    public interface IMongoDbService
    {
        void InsertStatistics(ServerStatistics stats);
    }

}
