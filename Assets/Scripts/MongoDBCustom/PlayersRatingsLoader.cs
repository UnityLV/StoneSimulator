using System.Collections.Generic;
using InGameUI;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using UnityEngine.Serialization;

namespace MongoDBCustom
{
    public class PlayersRatingsLoader : MonoBehaviour
    {
        [FormerlySerializedAs("menuRatingListUI")] [FormerlySerializedAs("_ratingListUI")] [SerializeField] private RatingListUI ratingListUI;  
        private async void Start()
        {
            var collection = MongoDBConnectionDataHolder.Data.Collection;

            var filter = Builders<BsonDocument>.Filter.Empty;
            var sort = Builders<BsonDocument>.Sort.Descending("rating");
            var limit = 100;

            var cursor = await collection.Find(filter).Sort(sort).Limit(limit).ToCursorAsync();

            var playersData = await cursor.ToListAsync();

            var ratingPlayerDataList = new List<RatingPlayerData>();

            for (int i = 0; i < playersData.Count; i++)
            {
                var ratingPlayerData = new RatingPlayerData
                {
                    Name = playersData[i]["name"].AsString,
                    RatingNumber = (i + 1).ToString(),
                    PointsAmount = playersData[i]["rating"].AsInt32.ToString()
                };
                ratingPlayerDataList.Add(ratingPlayerData);
            }
            
            ratingListUI.SetData(ratingPlayerDataList);
        }
    }
}