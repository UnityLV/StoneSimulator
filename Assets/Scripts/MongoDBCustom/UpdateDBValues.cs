using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;

namespace MongoDBCustom
{
    public class UpdateDBValues
    {
        public static async Task UpdatePlayerRatingAsync(int newRating)
        {
            var filter = Filters.DeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Set(DBKeys.Rating, newRating);

            await MongoDBConnectionDataHolder.Data.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Player rating updated new is " + newRating);
        }
        
        public static async Task UpdatePlayerName(string newName)
        {
            var filter = Filters.DeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Set(DBKeys.Name, newName);

            await MongoDBConnectionDataHolder.Data.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Player name updated new is " + newName);
        }
    }
}