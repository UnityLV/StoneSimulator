using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDBCustom;
using NaughtyAttributes;
using PlayerData.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public interface ISlaveClickCollector
{
    event UnityAction<int> Collected;
    void CollectClicksFromReferrals();
} 

namespace MainMenuUI
{
    public class SlaveClicksCollector :ISlaveClickCollector
    {
        private IDBCommands _idbCommands;
        private ISlavesDataService _slavesData;
        private IClickDataService _clickDataService;

        public event UnityAction<int> Collected;

        [Inject]
        private void Construct(IDBCommands idbCommands, ISlavesDataService slavesData, IClickDataService clickDataService)
        {
            _idbCommands = idbCommands;
            _slavesData = slavesData;
            _clickDataService = clickDataService;
        }
        
        public async void CollectClicksFromReferrals()
        {
            List<string> slavesId = _slavesData.GetSlaves().Data.Select(d => d.DeviseId).ToList();
            List<BsonDocument> slavesBeforeCollect = await _idbCommands.CollectClicksToGiveReferrer(slavesId);// here we already remove clicks from slaves data 
            int collect = slavesBeforeCollect.Select(ToCollectClicks()).Sum();
            _clickDataService.AddClicks(collect);
            Collected?.Invoke(collect);
        }

        private Func<BsonDocument, int> ToCollectClicks()
        {
            return d => d[DBKeys.ClickToGiveReferrer].AsInt32;
        }
    }
}