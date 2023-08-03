using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom
{
    public interface IMongoConnection
    {
        IMongoClient Client { get; }
        IMongoCollection<BsonDocument> UsersCollection { get; }
        IMongoCollection<BsonDocument> ChatCollection { get; }
        IMongoCollection<BsonDocument> PinnedMessagesCollection { get; }
    }
}