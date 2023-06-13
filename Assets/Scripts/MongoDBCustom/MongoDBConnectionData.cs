using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom
{
    public readonly struct MongoDBConnectionData
    {
        public MongoDBConnectionData(MongoClient client, IMongoDatabase database, IMongoCollection<BsonDocument> collection)
        {
            Client = client;
            Database = database;
            Collection = collection;
        }

        public readonly MongoClient Client;
        public readonly IMongoDatabase Database;
        public readonly IMongoCollection<BsonDocument> Collection;
    }
}