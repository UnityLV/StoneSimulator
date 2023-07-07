using System.Collections.Generic;
using MainMenuUI;
using MongoDB.Bson;
using PlayerData.Interfaces;
using UnityEngine;

namespace PlayerData
{
    
    public class PlayerSlavesData : BasePlayerData<JsonSavableClass>, ISlavesDataService
    {
        private const string DATA_PATCH = "SlavesData";

        private SlavesData _slavesData;

        public PlayerSlavesData() : base(DATA_PATCH)
        {
            Load();
            _slavesData = new SlavesData(JsonUtility.FromJson<SavablePlayerSlavesData>(Data.Json));
        }


        public void SetSlaves(List<BsonDocument> slaves)
        {
            _slavesData = new SlavesData(slaves);
            Data.Json = JsonUtility.ToJson(new SavablePlayerSlavesData().Slaves = _slavesData.Data);
            Save();
        }

        public SlavesData GetSlaves()
        {
            return _slavesData;
        }
    }
}