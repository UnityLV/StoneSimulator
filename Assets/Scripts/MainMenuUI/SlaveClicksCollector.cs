using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameScene;
using MongoDB.Bson;
using MongoDBCustom;
using PlayerData.Interfaces;
using Zenject;

public interface ISlaveClickCollector
{
    void CollectClicksFromReferrals();
}

namespace MainMenuUI
{
    public class SlaveClicksCollector : ISlaveClickCollector
    {
        private IDBCommands _idbCommands;
        private ISlavesDataService _slavesData;
        private AbilityClicks _abilityClicks;

        [Inject]
        private void Construct(IDBCommands idbCommands, ISlavesDataService slavesData, AbilityClicks abilityClicks)
        {
            _idbCommands = idbCommands;
            _slavesData = slavesData;
            _abilityClicks = abilityClicks;
        }

        public async void CollectClicksFromReferrals()
        {
            int collect = await CalculateClicks();
            _abilityClicks.AddClicks(collect);
        }

        private async Task<int> CalculateClicks()
        {
            List<string> slavesId = _slavesData.GetSlaves().Data.Select(d => d.DeviseId).ToList();
            List<BsonDocument> slavesBeforeCollect = await _idbCommands.CollectClicksToGiveReferrer(slavesId); // here we already remove clicks from slaves data 
            int collect = slavesBeforeCollect.Select(ToCollectClicks()).Sum();
            return collect;
        }

        private Func<BsonDocument, int> ToCollectClicks()
        {
            return d => d[DBKeys.ClickToGiveReferrer].AsInt32;
        }
    }
}