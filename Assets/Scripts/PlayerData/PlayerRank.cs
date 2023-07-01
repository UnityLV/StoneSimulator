using System.Threading.Tasks;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

namespace PlayerData
{
    public class PlayerRank : IRankDataService
    {
        private IClickDataService _clickDataService;
        private RankData _data;

        

        [Inject]
        private void Construct(IClickDataService clickDataService,RankData rankData)
        {
            _clickDataService = clickDataService;
            _data = rankData;
        }

        public SingleRank GetMyRank()
        {
            return _data.GetRankByClicks(_clickDataService.GetAllClickCount());
        }

    }
}