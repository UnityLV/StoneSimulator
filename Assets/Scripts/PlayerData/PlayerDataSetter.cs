using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Installers;
using JetBrains.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDBCustom;
using NaughtyAttributes;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

namespace PlayerData
{
    public class PlayerDataSetter : MonoBehaviour
    {
        private INicknameDataService _nicknameData;
        private IClickDataService _clickDataService;
        private ISlavesDataService _slavesDataService;
        private IDBPlayerDataProvider _playerDataProvider;

        private IDBCommands _idbCommands;

        [Inject]
        private void Construct(INicknameDataService nicknameDataService, IClickDataService clickDataService,
            ISlavesDataService slavesDataService, IDBPlayerDataProvider playerDataProvider)
        {
            _nicknameData = nicknameDataService;
            _clickDataService = clickDataService;
            _slavesDataService = slavesDataService;
            _playerDataProvider = playerDataProvider;
        }

        public async Task SetData()
        {
            _idbCommands = ValuesFromBootScene.IdbCommands;

            var PlayerData = await _playerDataProvider.GetPlayerDataById();

            ValuesFromBootScene.PlayerData = PlayerData;

            SetNickname(PlayerData);
            SetClicks(PlayerData);
            await SetSlaves(PlayerData);
        }

     
        private async Task SetSlaves(BsonDocument PlayerData)
        {
            List<BsonDocument> slaves = await GetMySlavesData(PlayerData);
            _slavesDataService.SetSlaves(slaves);
        }

        private async Task<List<BsonDocument>> GetMySlavesData(BsonDocument PlayerData)
        {
            string[] slavesId = PlayerData[DBKeys.Referrals].AsBsonArray.Select(v => v.AsString).ToArray();
            return await _idbCommands.GetPlayersDataById(slavesId);
        }

        
        private void SetClicks(BsonDocument PlayerData)
        {
            _clickDataService.SetClickCount(PlayerData[DBKeys.AllClick].AsInt32);
        }

        private void SetNickname(BsonDocument PlayerData)
        {
            _nicknameData.SetNickname(PlayerData[DBKeys.Name].AsString);
        }
    }
}