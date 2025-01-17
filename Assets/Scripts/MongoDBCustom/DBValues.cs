﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatDB;
using ChatDB.PinMessage;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using static MongoDBCustom.DBKeys;

namespace MongoDBCustom
{
    public class IdbCommands : IDBCommands
    {
        private readonly IMongoConnection _connection;

        public IdbCommands(IMongoConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<PinMessageData>> GetAllPinnedMessageDatesAsync()
        {
            var filter = Builders<BsonDocument>.Filter.Exists(PinDate);

            //var projection = Builders<BsonDocument>.Projection.Include(PinDate);
            var pinnedMessages = await _connection.PinnedMessagesCollection.Find(filter).ToListAsync();

            var pinnedDates = pinnedMessages
                .Select(doc => new PinMessageData
                {
                    PinDate = DateTools.ConvertFromCustomFormat(doc[PinDate].AsString),
                    Message = ChatMessageConverter.ConvertToChatMessage(doc)
                })
                .ToList();

            return pinnedDates;
        }

        public async Task PinMessageAsync(PinMessageData data)
        {
            var pinnedMessageDocument = new BsonDocument
            {
                { DeviceID, data.Message.DeviceID },
                { Name, data.Message.PlayerNickname },
                { Timestamp, data.Message.Timestamp },
                { Message, data.Message.MessageText },
                { PinDate, DateTools.ConvertToCustomFormat(data.PinDate) }
            };

            await _connection.PinnedMessagesCollection.InsertOneAsync(pinnedMessageDocument);
        }

        public async Task<PinMessageData> GetPinnedMessageAsync()
        {
            DateTime date = DateTime.UtcNow.Date;

            var filter = Builders<BsonDocument>.Filter.Eq(PinDate, DateTools.ConvertToCustomFormat(date));
            BsonDocument pinnedMessageDocument =
                await _connection.PinnedMessagesCollection.Find(filter).FirstOrDefaultAsync();

            if (pinnedMessageDocument != null)
            {
                var message = ChatMessageConverter.ConvertToChatMessage(pinnedMessageDocument);
                return new PinMessageData { Message = message, PinDate = date };
            }

            return default;
        }

        public async Task InsertChatMessageAsync(ChatMessage chatMessage)
        {
            BsonDocument chatDocument = ConvertToBson(chatMessage);

            await _connection.ChatCollection.InsertOneAsync(chatDocument);
        }

        private static BsonDocument ConvertToBson(ChatMessage chatMessage)
        {
            var chatDocument = new BsonDocument
            {
                { DeviceID, chatMessage.DeviceID },
                { Name, chatMessage.PlayerNickname },
                { Timestamp, chatMessage.Timestamp },
                { Message, chatMessage.MessageText }
            };
            return chatDocument;
        }

        public async Task<List<BsonDocument>> GetLastChatMessagesAsync(int numMessages)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var sort = Builders<BsonDocument>.Sort.Descending(Timestamp);
            var bsonMessages =
                await _connection.ChatCollection.Find(filter).Sort(sort).Limit(numMessages).ToListAsync();
            return bsonMessages;
        }

        public async Task RemoveMeFromReferral()
        {
            BsonDocument data = await GetPlayerDataAsync();
            string deviceId = data[Referrer].AsString;

            var filter = Builders<BsonDocument>.Filter.Eq(DeviceID, deviceId);
            var update = Builders<BsonDocument>.Update.Pull(Referrals, DeviceInfo.GetDeviceId());
            var options = new UpdateOptions { IsUpsert = false };
            await _connection.UsersCollection.UpdateOneAsync(filter, update, options);
            Debug.Log("Removed device ID " + DeviceInfo.GetDeviceId() + " from player " + deviceId + " referrals.");
        }

        public async Task<List<BsonDocument>> CollectClicksToGiveReferrer(List<string> slavesId)
        {
            var filter = Builders<BsonDocument>.Filter.In(DeviceID, slavesId);
            var playersDataBeforeUpdate = await _connection.UsersCollection.Find(filter).ToListAsync();
            var update = Builders<BsonDocument>.Update.Set(ClickToGiveReferrer, 0);
            await _connection.UsersCollection.UpdateManyAsync(filter, update);

            return playersDataBeforeUpdate;
        }

        public async Task<List<BsonDocument>> GetPlayersDataById(params string[] id)
        {
            var filter = Builders<BsonDocument>.Filter.In(DeviceID, id);
            var documents = await _connection.UsersCollection.Find(filter).ToListAsync();
            return documents;
        }

        public async Task DeleteMyData()
        {
            var filter = Filters.MyDeviseIDFilter();
            await _connection.UsersCollection.DeleteOneAsync(filter);
        }

        public async Task TryAddMeAsReferralsTo(string referrer)
        {
            if (await IsMyReferrerExist())
            {
                return;
            }

            var filter = Filters.IDFilter(referrer);
            var update = Builders<BsonDocument>.Update.AddToSet(Referrals, DeviceInfo.GetDeviceId());

            await AddReferrerToMyData(referrer);
            await _connection.UsersCollection.UpdateOneAsync(filter, update);
            Debug.Log("Added as referral device ID " + DeviceInfo.GetDeviceId() + " to player " + referrer);
        }

        private async Task<bool> IsMyReferrerExist()
        {
            var filter = Builders<BsonDocument>.Filter.Eq(DeviceID, DeviceInfo.GetDeviceId()) &
                         (Builders<BsonDocument>.Filter.Exists(Referrer, false) |
                          Builders<BsonDocument>.Filter.Eq(Referrer, ""));
            var document = await _connection.UsersCollection.Find(filter).FirstOrDefaultAsync();
            return document != null;
        }

        private async Task AddReferrerToMyData(string newReferrer)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Set(Referrer, newReferrer);

            await _connection.UsersCollection.UpdateOneAsync(filter, update);
            Debug.Log("Referrer my set: " + newReferrer);
        }

        public async Task AddAllPlayerClicks(int add)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Inc(AllClick, add);

            await _connection.UsersCollection.UpdateOneAsync(filter, update);
            Debug.Log("All Player clicks db add " + add);
        }

        public async Task AddPlayerClickToGiveReferrer(int clicksToAdd)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update
                .Inc(ClickToGiveReferrer, clicksToAdd)
                .Inc(AllClickToGiveReferrer, clicksToAdd);

            await _connection.UsersCollection.UpdateOneAsync(filter, update);

            Debug.Log("Player Referrer Click updated " + clicksToAdd);
        }

        public async Task<BsonDocument> GetPlayerDataAsync()
        {
            var filter = Filters.MyDeviseIDFilter();
            BsonDocument playerData =
                await _connection.UsersCollection.Find(filter).FirstOrDefaultAsync();

            return playerData;
        }

        public async Task UpdatePlayerName(string newName)
        {
            var filter = Filters.MyDeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Set(Name, newName);

            await _connection.UsersCollection.UpdateOneAsync(filter, update);
            Debug.Log("Player name updated new is " + newName);
        }

        public async Task InsertPlayerDataAsync(BsonDocument playerData)
        {
            await _connection.UsersCollection.InsertOneAsync(playerData);
        }

        public async Task<List<BsonDocument>> PlayersRating()
        {
            var collection = _connection.UsersCollection;
            var filter = Builders<BsonDocument>.Filter.Empty;
            var sort = Builders<BsonDocument>.Sort.Descending(AllClick);
            var limit = 100;
            var cursor = await collection.Find(filter).Sort(sort).Limit(limit).ToCursorAsync();
            var playersData = await cursor.ToListAsync();
            return RemoveInvalidUsers(playersData);
        }

        private List<BsonDocument> RemoveInvalidUsers(List<BsonDocument> users)
        {
            List<BsonDocument> validUsers = new();

            foreach (var user in users)
            {
                if (user.Contains(Name))
                {
                    validUsers.Add(user);
                }
            }

            return validUsers;
        }

        public async Task<int> GetLocationHealth()
        {
            const string dataDeviceId = "ServerDataHolder";
            var filter = Builders<BsonDocument>.Filter.Eq(DeviceID, dataDeviceId);
            BsonDocument playerData = await _connection.UsersCollection.Find(filter).FirstOrDefaultAsync();
            return playerData["HealthPerLevel"].AsInt32;
        }
    }
}