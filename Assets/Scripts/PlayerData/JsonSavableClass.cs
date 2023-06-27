using System;

namespace PlayerData
{
    [Serializable]
    public class JsonSavableClass : ISaveble
    {
        public string Json = "{}";
    }
}