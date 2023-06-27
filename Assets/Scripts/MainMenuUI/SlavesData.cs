using System;
using System.Collections.Generic;
using System.Linq;
using InGameUI;
using MongoDB.Bson;
using MongoDBCustom;
using PlayerData;
using PlayerData.Interfaces;

namespace MainMenuUI
{
    public class SlavesData
    {
        public List<SingleSlaveData> Data { get; private set; } = new List<SingleSlaveData>();
        public int AllClicks { get; private set; }
        public int AllSlaves { get; private set; }

        public SlavesData(List<BsonValue> data)
        {
            Data = ToSlaveData(data);
            AllClicks = Data.Select(d => d.Clicks).Sum();
            AllSlaves = Data.Count();
        }
        
        public SlavesData(SavablePlayerSlavesData savable)
        {
            Data = savable.Data;
            AllClicks = Data.Select(d => d.Clicks).Sum();
            AllSlaves = Data.Count();
        }

        private List<SingleSlaveData> ToSlaveData(List<BsonValue> data)
        {
            return data.Select(ToSlaveData).ToList();
        }

        private SingleSlaveData ToSlaveData(BsonValue document)
        {
            return new SingleSlaveData
                { Name = document[DBKeys.Name].AsString, Clicks = document[DBKeys.AllClickToGiveReferrer].AsInt32 };
        }
    }
}