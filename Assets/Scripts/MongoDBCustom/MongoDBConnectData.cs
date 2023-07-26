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
            Debug.Log("Connection string " + _config.GetConnection());

            IMongoClient client = new MongoClient( _config.GetConnection());
            
            IMongoDatabase database = client.GetDatabase(DBKeys.DataBase);
            
            IMongoCollection<BsonDocument> playerCollection = database.GetCollection<BsonDocument>(DBKeys.Collection);
            
            MongoMongoConnectionData connectionData = new MongoMongoConnectionData(client, playerCollection);

            return connectionData;
        }
    }
}