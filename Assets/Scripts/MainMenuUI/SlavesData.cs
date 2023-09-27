using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDBCustom;
using PlayerData;
using PlayerData.Interfaces;

namespace MainMenuUI
{
    public static class BsonExtension
    {
        public static List<SingleSlaveData> ToSlaveData(this List<BsonDocument> data)
        {
            return data.Select(ToSlaveData).ToList();
        }

        private static SingleSlaveData ToSlaveData(BsonDocument document)
        {
            return new SingleSlaveData
            {
                Name = document[DBKeys.Name].AsString, Clicks = document[DBKeys.AllClickToGiveReferrer].AsInt32,
                DeviseId = document[DBKeys.DeviceID].AsString
            };
        }
    }

    public class SlavesData
    {
        public List<SingleSlaveData> Data { get; private set; } = new List<SingleSlaveData>();
        public int AllClicks { get; private set; }
        public int AllSlaves { get; private set; }

        public SlavesData(List<BsonDocument> data)
        {
            Data = data.ToSlaveData();
            AllClicks = Data.Select(d => d.Clicks).Sum();
            AllSlaves = Data.Count();
        }

        public SlavesData(SavablePlayerSlavesData savable)
        {
            Data = savable.Slaves;
            AllClicks = Data.Select(d => d.Clicks).Sum();
            AllSlaves = Data.Count();
        }
    }
}