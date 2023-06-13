using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom
{
    public static class DBKeys
    {
        public const string DataBase = "mongo";
        public const string Collection = "users";
        public const string DeviceID = "deviceId";
        public const string PlayerId = DeviceID;
        public const string Rating = "Rating";
    }
}