using System;
namespace ChatDB.PinMessage
{
    
    
    public struct PinMessageData
    {
        public ChatMessage Message;
        public DateTime PinDate;

        public override string ToString()
        {
            return $"{Message} @ {PinDate}";
        }
    }
}