using System;
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
        private IDBValues _dbValues;

        [Inject]
        private void Construct(IClickDataService clickDataService)
        {
            _clickDataService = clickDataService;
         
        }
        
        public async Task<BsonDocument> GetPlayerDataByIdAsync()
        {
            _dbValues = ValuesFromBootScene.DBValues;
            BsonDocument playerData = await _dbValues.GetPlayerDataAsync();

            if (playerData == null)
            {
                playerData = CreateFirstPlayerData();
                await _dbValues.InsertPlayerDataAsync(playerData);
                
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
                { DBKeys.Name, String.Empty },
                { DBKeys.AllClick, 0 },
                { DBKeys.ClickToGiveReferrer, 500},
                { DBKeys.AllClickToGiveReferrer, 500},
                { DBKeys.Referrals, new BsonArray() },
            };
            

            return playerData;
        }

    }
}