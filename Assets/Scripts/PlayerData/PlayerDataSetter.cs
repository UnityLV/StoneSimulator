﻿using System;
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
        
        private IDBValues _dbValues;

        public RefferalTest _Refferal;


        [Inject]
        private void Construct(INicknameDataService nicknameDataService, IClickDataService clickDataService,
            ISlavesDataService slavesDataService,IDBPlayerDataProvider playerDataProvider)
        {
            _nicknameData = nicknameDataService;
            _clickDataService = clickDataService;
            _slavesDataService = slavesDataService;
            _playerDataProvider = playerDataProvider;
        }

        public async Task SetData()
        {
            _dbValues = ValuesFromBootScene.DBValues;

            var PlayerData = await _playerDataProvider.GetPlayerDataByIdAsync();

            SetNickname(PlayerData);
            SetClicks(PlayerData);
            
            List<BsonDocument> slaves = await GetMySlavesData(PlayerData);
            SetSlaves(slaves);

            _Refferal._dbValues = _dbValues;
            _Refferal._connection = ValuesFromBootScene.MongoConnection;
        }

        private async Task<List<BsonDocument>> GetMySlavesData(BsonDocument PlayerData)
        {
            List<string> slavesId = PlayerData[DBKeys.Referrals].AsBsonArray.Select(v => v.AsString).ToList();
            return await _dbValues.GetPlayersDataById(slavesId);
        }

        private void SetSlaves(List<BsonDocument> slaves)
        {
            _slavesDataService.SetSlaves(slaves);
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