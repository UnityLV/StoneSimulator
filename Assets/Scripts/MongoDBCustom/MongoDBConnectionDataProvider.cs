using System;
using System.Threading.Tasks;
using FirebaseCustom;
using UnityEngine;
using UnityEngine.Events;


namespace MongoDBCustom
{
    public class MongoDBConnectionDataProvider : MonoBehaviour, IDBConnectionProvider
    {
        [SerializeField] private MongoDBConnectionConfig _dbConnectionConfig;

        private MongoDBConnector _connector;
        private MongoDBConnectionData _connection;


        public event UnityAction<MongoDBConnectionData> SuccessConnect;
        public event UnityAction<Exception> FailedConnect;

        private void Awake()
        {
            _connector = new MongoDBConnector(_dbConnectionConfig);
            _connector.Connected += OnConnected;
            _connector.Connection();
        }

        private void OnDestroy()
        {
            _connector.Connected -= OnConnected;
        }

        private async void OnConnected(MongoDBConnectionData connection)
        {
            _connection = connection;

            await TryConnect();
        }

        private async Task TryConnect()
        {
            try
            {
                await TestConnection();
                HandleSuccessConnect();
            }
            catch (Exception ex)
            {
                await HandleFailConnect(ex);
            }
        }

        private async Task TestConnection()
        {
            Debug.Log("Connect To DataBase");
            await _connection.Client.ListDatabasesAsync();
        }

        private void HandleSuccessConnect()
        {
            Debug.Log($"Data Base SuccessConnect");
            SuccessConnect?.Invoke(_connection);
        }

        private async Task HandleFailConnect(Exception ex)
        {
            Debug.LogError($"Data Base FailedConnect {ex.Message}");
            FailedConnect?.Invoke(ex);

            Debug.Log("Try Connect");
            await TryConnect();
        }
    }
}