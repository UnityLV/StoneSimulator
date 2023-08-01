using System;
namespace ChatDB
{
    public struct ChatMessage
    {
        public string DeviceID { get; set; }
        public string PlayerNickname { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageText { get; set; }

        public override string ToString()
        {
            return $" DeviceID: {DeviceID}\n PlayerNickname: {PlayerNickname}\n Timestamp: {Timestamp}\n MessageText: {MessageText}";
        }
    }
}