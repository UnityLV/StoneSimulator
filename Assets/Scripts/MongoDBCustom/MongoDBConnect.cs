using System.IO;
using FirebaseCustom;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using UnityEngine.Events;


namespace MongoDBCustom
{
    public class MongoDBConnect
    {
        private MongoDBConnectionConfig _config;

        public MongoDBConnect(MongoDBConnectionConfig config)
        {
            _config = config;
        }

        public MongoConnectionData GetConnectionData()
        {
            IMongoClient client = new MongoClient( _config.GetConnection());
            
            IMongoDatabase database = client.GetDatabase(DBKeys.DataBase);
            
            IMongoCollection<BsonDocument> playerCollection = database.GetCollection<BsonDocument>(DBKeys.UsersCollection);
            IMongoCollection<BsonDocument> chatCollection = database.GetCollection<BsonDocument>(DBKeys.ChatCollection);
            IMongoCollection<BsonDocument> pinnedMessagesCollection = database.GetCollection<BsonDocument>(DBKeys.PinnedMessagesCollection);
            
            MongoConnectionData connectionData = new MongoConnectionData(client, playerCollection,chatCollection,pinnedMessagesCollection);

            return connectionData;
        }
    }
}