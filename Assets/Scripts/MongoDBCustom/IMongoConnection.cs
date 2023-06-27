using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom
{
    public interface IMongoConnection
    {
        IMongoClient Client { get; }
        IMongoCollection<BsonDocument> Collection { get; }
    }
}