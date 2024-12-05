using Consumer.property;
using MongoDB.Driver;

namespace Consumer.MongoDB
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoCollection<ServerStatistics> _collection;

        public MongoDbService(MongoDbConfig mongoDbConfig)
        {
            var client = new MongoClient(mongoDbConfig.ConnectionString);
            var database = client.GetDatabase(mongoDbConfig.DatabaseName);
            _collection = database.GetCollection<ServerStatistics>(mongoDbConfig.CollectionName);
        }

        public void InsertStatistics(ServerStatistics stats)
        {
            _collection.InsertOne(stats);
        }
    }

}
