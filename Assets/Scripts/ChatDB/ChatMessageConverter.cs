using System.Collections.Generic;
using MongoDB.Bson;
using static MongoDBCustom.DBKeys;
namespace ChatDB
{
    public class ChatMessageConverter
    {
        public static List<ChatMessage> ConvertToChatMessages(List<BsonDocument> bsonMessages)
        {
            List<ChatMessage> chatMessages = new List<ChatMessage>();
            foreach (var bsonMessage in bsonMessages)
            {
                ChatMessage chatMessage = ConvertToChatMessage(bsonMessage);

                chatMessages.Add(chatMessage);
            }

            return chatMessages;
        }

        public static ChatMessage ConvertToChatMessage(BsonDocument bsonMessage)
        {
            ChatMessage chatMessage = new ChatMessage {
                DeviceID = bsonMessage[DeviceID].AsString,
                PlayerNickname = bsonMessage[Name].AsString,
                Timestamp = bsonMessage[Timestamp].ToUniversalTime(),
                MessageText = bsonMessage[Message].AsString
            };
            return chatMessage;
        }
    }
}