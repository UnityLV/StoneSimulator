using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom 
{
    public static class Filters
    {
        public static FilterDefinition<BsonDocument> MyDeviseIDFilter()
        {
            var filter = Builders<BsonDocument>.Filter.Eq(DBKeys.DeviceID, DeviceInfo.GetDeviceId());
            return filter;
        }
        
        public static FilterDefinition<BsonDocument> IDFilter(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(DBKeys.DeviceID, id);
            return filter;
        }
    }
}