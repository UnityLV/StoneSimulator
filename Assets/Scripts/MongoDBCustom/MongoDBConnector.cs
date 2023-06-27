using System;
using System.Threading;
using System.Threading.Tasks;
using FirebaseCustom;
using UnityEngine;
using UnityEngine.Events;


namespace MongoDBCustom
{
    public class MongoDBConnector : IDBConnector
    {

        private IMongoConnection _connection;

        public MongoDBConnector(IMongoConnection connectData)
        {
            _connection = connectData;
        }

        public async Task TryConnect()
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
            await _connection.Client.StartSessionAsync();
        }

        private void HandleSuccessConnect()
        {
            Debug.Log($"Data Base SuccessConnect");
        }

        private async Task HandleFailConnect(Exception ex)
        {
            Debug.LogError($"Data Base FailedConnect {ex.Message}");
            await Task.Delay(100);
            Debug.Log("Try Connect");
            await TryConnect();
        }
        
      
    }
}