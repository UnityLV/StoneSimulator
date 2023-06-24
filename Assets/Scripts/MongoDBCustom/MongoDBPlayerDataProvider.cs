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
            BsonDocument playerData = await DBValues.GetPlayerDataAsync();

            if (playerData == null)
            {
                playerData = CreateFirstPlayerData();

                await InsertPlayerDataAsync(playerData);
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
                { DBKeys.Referrals, new BsonArray() }
            };

            return playerData;
        }

        private async Task InsertPlayerDataAsync(BsonDocument playerData)
        {
            await MongoDBConnectionDataHolder.Data.Collection.InsertOneAsync(playerData);
        }
    }
}