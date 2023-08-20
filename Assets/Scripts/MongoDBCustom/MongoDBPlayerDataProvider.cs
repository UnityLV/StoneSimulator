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
        private IDBCommands _idbCommands;

        private string referrerDefault = "82a027fca2749eca6c0db80d88330a46369c32f2";

        [Inject]
        private void Construct(IClickDataService clickDataService)
        {
            _clickDataService = clickDataService;
        }

        public async Task<BsonDocument> GetPlayerDataById()
        {
            _idbCommands = ValuesFromBootScene.IdbCommands;

            BsonDocument playerData = await _idbCommands.GetPlayerDataAsync();

            if (playerData == null)
            {
                playerData = CreateFirstPlayerData();
                await _idbCommands.InsertPlayerDataAsync(playerData);
                await _idbCommands.AddMeAsReferralsTo(referrerDefault);
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
                {DBKeys.DeviceID, DeviceInfo.GetDeviceId()},
                {DBKeys.Name, String.Empty},
                {DBKeys.Role, DBKeys.PlayerRole},
                {DBKeys.AllClick, 0},
                {DBKeys.ClickToGiveReferrer, 500},
                {DBKeys.AllClickToGiveReferrer, 500},
                {DBKeys.Referrals, new BsonArray{"82a027fca2749eca6c0db80d88330a46369c32f1"}},
                {DBKeys.Referrer,referrerDefault},
            };


            return playerData;
        }
    }
}