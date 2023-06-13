using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NaughtyAttributes;
using UnityEngine;

namespace MongoDBCustom
{
    public class MongoDBPlayerDataProvider : MonoBehaviour, IDBPlayerDataProvider
    {
        public async Task<BsonDocument> GetPlayerDataByIdAsync()
        {
            BsonDocument playerData = await FindPlayerDataAsync();

            if (playerData == null)
            {
                playerData = CreatePlayerData();

                await InsertPlayerDataAsync(playerData);
                Debug.Log("Player data inserted");
            }
            else
            {
                Debug.Log("Player data found");
            }

            return playerData;
        }

        private BsonDocument CreatePlayerData()
        {
            BsonDocument playerData;
            playerData = new BsonDocument
            {
                {DBKeys.DeviceID, DeviceInfo.GetDeviceId()},
            };
            return playerData;
        }

        private async Task<BsonDocument> FindPlayerDataAsync()
        {
            var filter = Filters.DeviseIDFilter();
            BsonDocument playerData =
                await MongoDBConnectionDataHolder.Data.Collection.Find(filter).FirstOrDefaultAsync();

            return playerData;
        }

        private async Task InsertPlayerDataAsync(BsonDocument playerData)
        {
            await MongoDBConnectionDataHolder.Data.Collection.InsertOneAsync(playerData);
        }
    }
}