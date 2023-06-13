using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine.Events;


namespace MongoDBCustom
{
    public class MongoDBConnector
    {
        private MongoDBConnectionConfig _config;

        public MongoDBConnector(MongoDBConnectionConfig config)
        {
            _config = config;
        }

        public event UnityAction<MongoDBConnectionData> Connected;

        public void Connection()
        {
            MongoClient client = new MongoClient(_config.GetConnectionString());
            IMongoDatabase database = client.GetDatabase(DBKeys.DataBase);
            IMongoCollection<BsonDocument> playerCollection = database.GetCollection<BsonDocument>(DBKeys.Collection);

            MongoDBConnectionData connectionData = new MongoDBConnectionData(client, database, playerCollection);

            Connected?.Invoke(connectionData);
        }
    }
}