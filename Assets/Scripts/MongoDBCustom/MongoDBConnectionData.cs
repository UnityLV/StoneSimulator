using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom
{
    public readonly struct MongoDBConnectionData
    {
        public MongoDBConnectionData(IMongoClient client, IMongoDatabase database, IMongoCollection<BsonDocument> collection)
        {
            Client = client;
            Database = database;
            Collection = collection;
        }

        public readonly IMongoClient Client;
        public readonly IMongoDatabase Database;
        public readonly IMongoCollection<BsonDocument> Collection;
    }
}