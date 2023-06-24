using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;

namespace MongoDBCustom
{
    public class DBValues
    {
        
        public static List<string> GetMyReferralsID()
        {
            var filter = Filters.MyDeviseIDFilter();
            var document = MongoDBConnectionDataHolder.Data.Collection.Find(filter).FirstOrDefault();

            if (document != null && document.Contains(DBKeys.Referrals))
            {
                var referrals = document[DBKeys.Referrals].AsBsonArray;
                return referrals.Select(r => r.ToString()).ToList();
            }

            return new List<string>();
        }
        
        public static async Task<List<BsonDocument>> GetReferralPlayersAsync()
        {
            var filter = Builders<BsonDocument>.Filter.In(DBKeys.DeviceID, GetMyReferralsID());
            var documents = await MongoDBConnectionDataHolder.Data.Collection.Find(filter).ToListAsync();

            return documents;
        }
        
        public static async Task SetMeAsRefferalTo(string toRefferalDeviseId)
        {
            var filter = Filters.IDFilter(toRefferalDeviseId);
            var update = Builders<BsonDocument>.Update.AddToSet(DBKeys.Referrals, DeviceInfo.GetDeviceId());

            await MongoDBConnectionDataHolder.Data.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Referral added: to" + toRefferalDeviseId);
        }

        public static async Task UpdateAllPlayerClicksAsync(int newClicks)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Set(DBKeys.AllClick, newClicks);

            await MongoDBConnectionDataHolder.Data.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Player clicks updated new is " + newClicks);
        }
        
        
        public static async Task<BsonDocument> GetPlayerDataAsync()
        {
            var filter = Filters.MyDeviseIDFilter();
            BsonDocument playerData =
                await MongoDBConnectionDataHolder.Data.Collection.Find(filter).FirstOrDefaultAsync();

            return playerData;
        }

        
        public static async Task UpdatePlayerName(string newName)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Set(DBKeys.Name, newName);

            await MongoDBConnectionDataHolder.Data.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Player name updated new is " + newName);
        }
        
        
        public static async Task<List<BsonDocument>> PlayersRating()
        {
            var collection = MongoDBConnectionDataHolder.Data.Collection;
            var filter = Builders<BsonDocument>.Filter.Empty;
            var sort = Builders<BsonDocument>.Sort.Descending(DBKeys.AllClick);
            var limit = 100;
            var cursor = await collection.Find(filter).Sort(sort).Limit(limit).ToCursorAsync();
            var playersData = await cursor.ToListAsync();
            return playersData;
        }
    }
}