using System.Threading.Tasks;
using Installers;
using MongoDB.Bson;
using MongoDB.Driver;
using NaughtyAttributes;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

namespace MongoDBCustom
{
    public class MongoDBPlayerDataProvider : IDBPlayerDataProvider
    {
        private IClickDataService _clickDataService;
        private IMongoConnection _connection;
        private IDBValues _dbValues;

        [Inject]
        private void Construct(IClickDataService clickDataService)
        {
            _clickDataService = clickDataService;
        }
        
        public async Task<BsonDocument> GetPlayerDataByIdAsync()
        {
            BsonDocument playerData = await _dbValues.GetPlayerDataAsync();

            if (playerData == null)
            {
                playerData = CreateFirstPlayerData();
                await InsertPlayerDataAsync(playerData);
                
                _clickDataService.ResetAll();
                
                Debug.Log("Player data inserted");
            }
            else
            {
                Debug.Log("Player data found");
            }

            return playerData;
        }

        private BsonDocument CreateFirstPlayerData()
        {
            BsonDocument playerData = new BsonDocument
            {
                { DBKeys.DeviceID, DeviceInfo.GetDeviceId() },
                { DBKeys.Name, "name has not been set yet" },
                { DBKeys.AllClick, 0 },
                { DBKeys.ClickToGiveReferrer, 0 },
                { DBKeys.AllClickToGiveReferrer, 0 },
                { DBKeys.Referrals, new BsonArray() },
            };
            

            return playerData;
        }

        private async Task InsertPlayerDataAsync(BsonDocument playerData)
        {
            await _connection.Collection.InsertOneAsync(playerData);
        }
    }
}