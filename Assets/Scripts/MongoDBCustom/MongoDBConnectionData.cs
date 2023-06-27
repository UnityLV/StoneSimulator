using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom
{
    public class MongoMongoConnectionData : IMongoConnection
    {
        public MongoMongoConnectionData(IMongoClient client, IMongoCollection<BsonDocument> collection)
        {
            Client = client;
            Collection = collection;
        }

        public IMongoClient Client { get; }
        public IMongoCollection<BsonDocument> Collection { get; }
    }
}