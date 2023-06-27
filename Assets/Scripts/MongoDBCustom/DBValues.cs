using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using Zenject;

namespace MongoDBCustom
{
    public class DBValues : IDBValues
    {
        private readonly IMongoConnection _connection;

        public DBValues(IMongoConnection connection)
        {
            _connection = connection;
        }

        // public async Task<List<string>> GetMyReferralsID()
        // {
        //     var filter = Filters.MyDeviseIDFilter();
        //     var document = await _connection.Collection.Find(filter).FirstOrDefaultAsync();
        //
        //     if (document != null && document.Contains(DBKeys.Referrals))
        //     {
        //         var referrals = document[DBKeys.Referrals].AsBsonArray;
        //         return referrals.Select(r => r.ToString()).ToList();
        //     }
        //
        //     return new List<string>();
        // }
        //
        // public  async Task<List<BsonDocument>> GetReferralPlayers()
        // {
        //     var filter = Builders<BsonDocument>.Filter.In(DBKeys.DeviceID, await GetMyReferralsID());
        //     var documents = await _connection.Collection.Find(filter).ToListAsync();
        //
        //     return documents;
        // }

        public async Task SetMeAsRefferalTo(string toRefferalDeviseId)
        {
            var filter = Filters.IDFilter(toRefferalDeviseId);
            var update = Builders<BsonDocument>.Update.AddToSet(DBKeys.Referrals, DeviceInfo.GetDeviceId());

            await _connection.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Referral added: to" + toRefferalDeviseId);
        }

        public async Task AddAllPlayerClicks(int add)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Inc(DBKeys.AllClick, add);

            await _connection.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Player clicks updated new is " + add);
        }

        public async Task AddPlayerClickToGiveReferrer(int clicksToAdd)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update
                .Inc(DBKeys.ClickToGiveReferrer, clicksToAdd)
                .Inc(DBKeys.AllClickToGiveReferrer, clicksToAdd);

            await _connection.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Player Click To Give Referrer updated " + clicksToAdd);
        }


        public async Task<BsonDocument> GetPlayerDataAsync()
        {
            var filter = Filters.MyDeviseIDFilter();
            BsonDocument playerData =
                await _connection.Collection.Find(filter).FirstOrDefaultAsync();

            return playerData;
        }


        public async Task UpdatePlayerName(string newName)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Set(DBKeys.Name, newName);

            await _connection.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Player name updated new is " + newName);
        }


        public async Task<List<BsonDocument>> PlayersRating()
        {
            var collection = _connection.Collection;
            var filter = Builders<BsonDocument>.Filter.Empty;
            var sort = Builders<BsonDocument>.Sort.Descending(DBKeys.AllClick);
            var limit = 100;
            var cursor = await collection.Find(filter).Sort(sort).Limit(limit).ToCursorAsync();
            var playersData = await cursor.ToListAsync();
            return playersData;
        }
    }
}