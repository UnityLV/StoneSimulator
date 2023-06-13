using MongoDB.Bson;
using MongoDB.Driver;
using NaughtyAttributes;
using UnityEngine;

namespace MongoDBCustom
{
    public class MongoDBPlayerDataExample : MonoBehaviour
    {
        [ResizableTextArea] [SerializeField] private string _jsonExample;

        private void OnEnable()
        {
            _jsonExample = (GetPlayerDataById().ToString());
        }

        private BsonDocument GetPlayerDataById()
        {
            BsonDocument playerData = FindPlayerData();

            if (playerData == null)
            {
                playerData = new BsonDocument
                {
                    {DBKeys.DeviceID, DeviceInfo.GetDeviceId()},
                };

                InsertPlayerData(playerData);
                Debug.Log("Player data insert");
            }
            else
            {
                Debug.Log("Player data find");
            }

            return playerData;
        }

        public BsonDocument FindPlayerData()
        {
            var filter = Filters.DeviseIDFilter();
            BsonDocument playerData = MongoDBConnectionDataHolder.Data.Collection.Find(filter).FirstOrDefault();

            return playerData;
        }

        public void InsertPlayerData(BsonDocument playerData)
        {
            MongoDBConnectionDataHolder.Data.Collection.InsertOne(playerData);
        }
    }
}