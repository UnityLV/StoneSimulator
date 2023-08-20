using System;
using System.Collections.Generic;
using System.Linq;
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
        private IDBCommands _idbCommands;
        private INicknameDataService _nicknameDataService;


        [Inject]
        public void Construct(IDBCommands idbCommands, INicknameDataService nicknameDataService)
        {
            _idbCommands = idbCommands;
            _nicknameDataService = nicknameDataService;
        }

        public async Task<List<ChatMessage>> GetLastChatMessagesAsync(int numMessages = 30)
        {
            List<BsonDocument> bsonMessages = await _idbCommands.GetLastChatMessagesAsync(numMessages);
            List<ChatMessage> chatMessages =  ChatMessageConverter.ConvertToChatMessages(bsonMessages);

            return chatMessages.OrderBy(message => message.Timestamp).ToList();
        }

        public async Task InsertMessage(string message)
        {
            ChatMessage chatMessage = ConvertToChatMessage(message);

            await _idbCommands.InsertChatMessageAsync(chatMessage);
        }

        public ChatMessage ConvertToChatMessage(string message)
        {
            ChatMessage chatMessage = new ChatMessage {
                DeviceID = DeviceInfo.GetDeviceId(),
                PlayerNickname = _nicknameDataService.GetNickname(),
                Timestamp = DateTime.UtcNow,
                MessageText = message
            };
            return chatMessage;
        }
    }
}