using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBCustom;
using PlayerData.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;
using static MongoDBCustom.DBKeys;

namespace ChatDB
{
    public class ChatDB : MonoBehaviour
    {
        private IDBValues _dbValues;
        private INicknameDataService _nicknameDataService;

        private readonly ChatMessageConverter _messageConverter = new ChatMessageConverter();
        
     
        
        [Inject]
        public void Construct(IDBValues dbValues, INicknameDataService nicknameDataService)
        {
            _dbValues = dbValues;
            _nicknameDataService = nicknameDataService;
        }

        public async Task<List<ChatMessage>> GetLastChatMessagesAsync(int numMessages = 20)
        {
            List<BsonDocument> bsonMessages = await _dbValues.GetLastChatMessagesAsync(numMessages);
            return _messageConverter.ConvertToChatMessages(bsonMessages);
        }

        public async Task InsertMessage(string message)
        {
            ChatMessage chatMessage = new ChatMessage {
                DeviceID = DeviceInfo.GetDeviceId(),
                PlayerNickname = _nicknameDataService.GetNickname(),
                Timestamp = DateTime.UtcNow,
                MessageText = message
            };
            
            await _dbValues.InsertChatMessageAsync(chatMessage);
        }

    }
}