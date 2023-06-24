using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NaughtyAttributes;
using UnityEngine;

namespace MongoDBCustom
{
    public class RefferalTest : MonoBehaviour
    {
        
        [Button()]
        public static async Task AddRandomPlayerToReferralsAsync()
        {
            var filter = Builders<BsonDocument>.Filter.Ne(DBKeys.DeviceID, DeviceInfo.GetDeviceId());
            var documents = await MongoDBConnectionDataHolder.Data.Collection.Find(filter).ToListAsync();

            if (documents.Count > 0)
            {
                var randomIndex = UnityEngine.Random.Range(0, documents.Count);
                var randomPlayer = documents[randomIndex];
                var referralDeviceId = randomPlayer[DBKeys.DeviceID].AsString;

                var myFilter = Filters.MyDeviseIDFilter();
                var update = Builders<BsonDocument>.Update.AddToSet(DBKeys.Referrals, referralDeviceId);

                await MongoDBConnectionDataHolder.Data.Collection.UpdateOneAsync(myFilter, update);
                Debug.Log("Random player added to referrals: " + referralDeviceId);
            }
            else
            {
                Debug.Log("No players found in database");
            }
        }
        
        [Button()]
        public static async Task ClearDatabaseAsync()
        {
            var filter = new BsonDocument();
            await MongoDBConnectionDataHolder.Data.Collection.DeleteManyAsync(filter);
            Debug.Log("Database cleared");
        }
        [Button()]
        
        public static async Task PopulateDatabaseWithRealPlayersAsync()
        {
            int count = 20;
            var players = new List<BsonDocument>();

            var existingPlayers = await MongoDBConnectionDataHolder.Data.Collection.Find(new BsonDocument()).ToListAsync();

            for (int i = 0; i < count; i++)
            {
                var deviceId = GenerateRandomDeviceId();
                var name = "Test Player " + (i + 1);
                var allClicks = UnityEngine.Random.Range(0, 100);
                var referrals = GenerateRealReferrals(existingPlayers, deviceId);

                var player = new BsonDocument
                {
                    { DBKeys.DeviceID, deviceId },
                    { DBKeys.Name, name },
                    { DBKeys.AllClick, allClicks },
                    { DBKeys.Referrals, new BsonArray(referrals) }
                };

                players.Add(player);
            }

            await MongoDBConnectionDataHolder.Data.Collection.InsertManyAsync(players);
            Debug.Log("Real test players inserted into database");
        }

        private static List<string> GenerateRealReferrals(List<BsonDocument> existingPlayers, string excludeDeviceId)
        {
            var referrals = new List<string>();

            foreach (var player in existingPlayers)
            {
                var deviceId = player[DBKeys.DeviceID].AsString;

                if (deviceId != excludeDeviceId && !referrals.Contains(deviceId))
                {
                    referrals.Add(deviceId);
                }
            }

            return referrals;
        }
        
        private static string GenerateRandomDeviceId()
        {
            return Guid.NewGuid().ToString("N");
        }
        
        [Button()]
        public async void GetRefferals()
        {
            foreach (var VARIABLE in await DBValues.GetReferralPlayersAsync())
            {
                Debug.Log(VARIABLE);
            }
        }
        
        public void SetMeAsRefferal( string refferalId)
        {
            DBValues.SetMeAsRefferalTo(refferalId);
        }
    }
}