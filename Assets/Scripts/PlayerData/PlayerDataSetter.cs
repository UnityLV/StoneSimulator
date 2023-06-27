using System;
using System.Threading.Tasks;
using Installers;
using MongoDBCustom;
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

        private IDBValues _dbValues;

        [Inject]
        private void Construct(INicknameDataService nicknameDataService, IClickDataService clickDataService,
            ISlavesDataService slavesDataService)
        {
            _nicknameData = nicknameDataService;
            _clickDataService = clickDataService;
            _slavesDataService = slavesDataService;
        }

        public async Task SetData()
        {
            _dbValues = ValuesFromBootScene.DBValues;

            var PlayerData = await _dbValues.GetPlayerDataAsync();
            _nicknameData.SetNickname(PlayerData[DBKeys.Name].AsString);
            _clickDataService.SetClickCount(PlayerData[DBKeys.AllClick].AsInt32);
            _slavesDataService.SetSlaves(PlayerData[DBKeys.Referrals].AsBsonArray.ToList());
            Debug.Log(_slavesDataService.GetSlaves());
        }
    }
}