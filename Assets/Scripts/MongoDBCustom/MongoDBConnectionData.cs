using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom
{
    public class MongoMongoConnectionData : IMongoConnection
    {
        public MongoMongoConnectionData(IMongoClient client, IMongoCollection<BsonDocument> collection, IMongoCollection<BsonDocument> chatCollection, IMongoCollection<BsonDocument> pinnedMessagesCollection)
        
        {
            Client = client;
            UsersCollection = collection;
            ChatCollection = chatCollection;
            PinnedMessagesCollection = pinnedMessagesCollection;
        }

        public IMongoClient Client { get; }
        public IMongoCollection<BsonDocument> UsersCollection { get; }
        public IMongoCollection<BsonDocument> ChatCollection { get; }
        public IMongoCollection<BsonDocument> PinnedMessagesCollection { get; }
    }
}