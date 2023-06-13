using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


namespace MongoDBCustom
{
    public class MongoDBProvider : MonoBehaviour, IDBProvider
    {
        [SerializeField] private MongoDBConnectionConfig _dbConnectionConfig;

        private MongoDBConnector _connector;

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
            try
            {
                await TestConnection(connection);
                HandleSuccessConnect(connection);
            }
            catch (Exception ex)
            {
                HandleFailConnect(ex);
            }
        }

        private async Task TestConnection(MongoDBConnectionData connection)
        {
            await connection.Client.ListDatabasesAsync();
        }

        private void HandleSuccessConnect(MongoDBConnectionData connection)
        {
            Debug.Log($"{nameof(MongoDBConnector)} SuccessConnect");
            SuccessConnect?.Invoke(connection);
            
        }

        private void HandleFailConnect(Exception ex)
        {
            Debug.LogError($"{nameof(MongoDBConnector)} FailedConnect {ex.Message}");
            FailedConnect?.Invoke(ex);
           
        }
    }
}