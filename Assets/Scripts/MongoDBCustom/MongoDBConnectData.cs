using System.IO;
using FirebaseCustom;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using UnityEngine.Events;


namespace MongoDBCustom
{
    public class MongoDBConnectData
    {
        private MongoDBConnectionConfig _config;

        public MongoDBConnectData(MongoDBConnectionConfig config)
        {
            _config = config;
        }

        public MongoMongoConnectionData GetConnectionData()
        {
            IMongoClient client = new MongoClient( _config.GetConnection());
            
            IMongoDatabase database = client.GetDatabase(DBKeys.DataBase);
            
            IMongoCollection<BsonDocument> playerCollection = database.GetCollection<BsonDocument>(DBKeys.UsersCollection);
            IMongoCollection<BsonDocument> chatCollection = database.GetCollection<BsonDocument>(DBKeys.ChatCollection);
            IMongoCollection<BsonDocument> pinnedMessagesCollection = database.GetCollection<BsonDocument>(DBKeys.PinnedMessagesCollection);
            
            MongoMongoConnectionData connectionData = new MongoMongoConnectionData(client, playerCollection,chatCollection,pinnedMessagesCollection);

            return connectionData;
        }
    }
}