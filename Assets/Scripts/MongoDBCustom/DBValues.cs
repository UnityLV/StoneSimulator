using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using Zenject;
using static MongoDBCustom.DBKeys;

namespace MongoDBCustom
{
    public class DBValues : IDBValues
    {
        private readonly IMongoConnection _connection;


        public DBValues(IMongoConnection connection)
        {
            _connection = connection;
        }

        public async Task RemoveMeFromReferral()
        {
            BsonDocument data = await GetPlayerDataAsync();
            string deviceId = data[Referrer].AsString;

            var filter = Builders<BsonDocument>.Filter.Eq(DeviceID, deviceId);
            var update = Builders<BsonDocument>.Update.Pull(Referrals, DeviceInfo.GetDeviceId());
            var options = new UpdateOptions {IsUpsert = false};
            await _connection.Collection.UpdateOneAsync(filter, update, options);
            Debug.Log("Removed device ID " + DeviceInfo.GetDeviceId() + " from player " + deviceId + " referrals.");
        }

        public async Task<List<BsonDocument>> CollectClicksToGiveReferrer(List<string> slavesId)
        {
            var filter = Builders<BsonDocument>.Filter.In(DeviceID, slavesId);
            var playersDataBeforeUpdate = await _connection.Collection.Find(filter).ToListAsync();
            var update = Builders<BsonDocument>.Update.Set(ClickToGiveReferrer, 0);
            await _connection.Collection.UpdateManyAsync(filter, update);

            return playersDataBeforeUpdate;
        }

        public async Task<List<BsonDocument>> GetPlayersDataById(params string[] id)
        {
            var filter = Builders<BsonDocument>.Filter.In(DeviceID, id);
            var documents = await _connection.Collection.Find(filter).ToListAsync();
            return documents;
        }

        public async Task DeleteMyData()
        {
            var filter = Filters.MyDeviseIDFilter();
            await _connection.Collection.DeleteOneAsync(filter);
        }

        public async Task AddMeAsReferralsTo(string newReferrer)
        {
            var filter = Filters.IDFilter(newReferrer);
            var update = Builders<BsonDocument>.Update.AddToSet(Referrals, DeviceInfo.GetDeviceId());

            await AddReferrerToMyData(newReferrer);
            await _connection.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Referral me added: to" + newReferrer);
        }

        private async Task AddReferrerToMyData(string newReferrer)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Set(Referrer, newReferrer);

            await _connection.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Referrer my set: " + newReferrer);
        }

        public async Task AddAllPlayerClicks(int add)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Inc(AllClick, add);

            await _connection.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Player clicks add " + add);
        }

        public async Task AddPlayerClickToGiveReferrer(int clicksToAdd)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update
                .Inc(ClickToGiveReferrer, clicksToAdd)
                .Inc(AllClickToGiveReferrer, clicksToAdd);

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
            var update = Builders<BsonDocument>.Update.Set(Name, newName);

            await _connection.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Player name updated new is " + newName);
        }

        public async Task InsertPlayerDataAsync(BsonDocument playerData)
        {
            await _connection.Collection.InsertOneAsync(playerData);
        }


        public async Task<List<BsonDocument>> PlayersRating()
        {
            var collection = _connection.Collection;
            var filter = Builders<BsonDocument>.Filter.Empty;
            var sort = Builders<BsonDocument>.Sort.Descending(AllClick);
            var limit = 100;
            var cursor = await collection.Find(filter).Sort(sort).Limit(limit).ToCursorAsync();
            var playersData = await cursor.ToListAsync();
            return playersData;
        }
    }
}