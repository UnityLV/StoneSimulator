using System;
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

        public async Task InsertPlayerDataAsync(BsonDocument playerData)
        {
            await _connection.Collection.InsertOneAsync(playerData);
        }
        
        public DBValues(IMongoConnection connection)
        {
            _connection = connection;
        }
      
        
        public async Task<List<BsonDocument>> CollectClicksToGiveReferrer(List<string> slavesId)
        {
            var filter = Builders<BsonDocument>.Filter.In(DBKeys.DeviceID, slavesId);
            var playersDataBeforeUpdate = await _connection.Collection.Find(filter).ToListAsync();
            var update = Builders<BsonDocument>.Update.Set(DBKeys.ClickToGiveReferrer, 0);
            await _connection.Collection.UpdateManyAsync(filter, update);

            return playersDataBeforeUpdate;
        }

        public async Task<List<BsonDocument>> GetPlayersDataById(IEnumerable<string> id)
        {
            var filter = Builders<BsonDocument>.Filter.In(DBKeys.DeviceID, id);
            var documents = await _connection.Collection.Find(filter).ToListAsync();
            return documents;
        }

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
            Debug.Log("Player clicks add " + add);
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