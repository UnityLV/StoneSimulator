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
            UpdateDBValues.UpdatePlayerRatingAsync(_clickDataService.GetClickCount());
        }
    }
}