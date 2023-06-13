using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

namespace MongoDBCustom
{
    public class MongoDBPlayerDataSaver : IDBRatingSaver
    {
        private IClickDataService _clickDataService;

        [Inject]
        private void Construct(IClickDataService clickDataService)
        {
            _clickDataService = clickDataService;
        }

        public void SaveRating()
        {
            UpdatePlayerRatingAsync(_clickDataService.GetClickCount());
        }

        public async Task UpdatePlayerRatingAsync(int newRating)
        {
            var filter = Filters.DeviseIDFilter();
            var update = Builders<BsonDocument>.Update.Set(DBKeys.Rating, newRating);

            await MongoDBConnectionDataHolder.Data.Collection.UpdateOneAsync(filter, update);
            Debug.Log("Player rating updated new is " + newRating);
        }
    }
}