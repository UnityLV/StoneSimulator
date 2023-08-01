using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom
{
    public class MongoMongoConnectionData : IMongoConnection
    {
        public MongoMongoConnectionData(IMongoClient client, IMongoCollection<BsonDocument> collection, IMongoCollection<BsonDocument> chatCollection)
        {
            Client = client;
            UsersCollection = collection;
            ChatCollection = chatCollection;
        }

        public IMongoClient Client { get; }
        public IMongoCollection<BsonDocument> UsersCollection { get; }
        public IMongoCollection<BsonDocument> ChatCollection { get; }
    }
}