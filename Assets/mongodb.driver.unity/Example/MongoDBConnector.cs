using System;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using UnityEngine.Events;


namespace MongoDBCustom
{
    public class MongoDBDataProvider : MonoBehaviour
    {
        [SerializeField] private MongoDBConnectionConfig _dbConnectionConfig;

        private MongoDBConnector _connector;

        public event UnityAction<IMongoCollection<BsonDocument>> SuccessConnect;
        public event UnityAction FailedConnect;

        private void Awake()
        {
            _connector = new MongoDBConnector(_dbConnectionConfig);
        }

        private void HandleSuccessConnect()
        {
            Debug.Log($"{nameof(MongoDBConnector)} {nameof(SuccessConnect)}");
        }

        private void HandleFailConnect(Exception ex)
        {
            FailedConnect?.Invoke();
            Debug.LogError($"{nameof(MongoDBConnector)} {nameof(FailedConnect)} {ex.Message}");
        }

        private async Task InitializeDatabaseConnection()
        {
            try
            {
                await client.ListDatabasesAsync();

                SuccessConnect?.Invoke(_playerCollection);
                Debug.Log($"{nameof(MongoDBConnector)} {nameof(SuccessConnect)}");
            }
            catch (Exception ex)
            {
                FailedConnect?.Invoke();
                Debug.LogError($"{nameof(MongoDBConnector)} {nameof(FailedConnect)} {ex.Message}");
            }
        }
    }

    public class MongoDBConnector
    {
        private MongoDBConnectionConfig _config;

        public MongoDBConnector(MongoDBConnectionConfig config)
        {
            _config = config;
        }

        public event UnityAction<IMongoCollection<BsonDocument>> Connected;

        public void InitializeDatabaseConnection()
        {
            var client = new MongoClient(_config.GetConnectionString());
            var database = client.GetDatabase(DBKeys.DataBase);
            var playerCollection = database.GetCollection<BsonDocument>(DBKeys.Collection);
            Connected?.Invoke(playerCollection);
        }
    }
}