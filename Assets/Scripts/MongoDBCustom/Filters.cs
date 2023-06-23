using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom 
{
    public static class Filters
    {
        public static FilterDefinition<BsonDocument> DeviseIDFilter()
        {
            var filter = Builders<BsonDocument>.Filter.Eq(DBKeys.DeviceID, DeviceInfo.GetDeviceId());
            return filter;
        }
    }
}