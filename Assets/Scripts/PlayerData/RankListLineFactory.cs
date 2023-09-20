using System;
using PlayerData.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayerData
{
    [Serializable]
    public class RankListLineFactory
    {
        [SerializeField] private RankData _rankData;
        [SerializeField] private RankLine _rankLinePrefab;
        [SerializeField] private RankLine _selfRankLinePrefab;
        [SerializeField] private Transform _linesParent;

        private IClickDataService _clickDataService;

        public void SetDep(IClickDataService clickDataService)
        {
            _clickDataService = clickDataService;
        }

        public RankLine CreateLine(SingleRank rank)
        {
            RankLine line = IsThisPlayerRank(rank) ? CreatePlayerRatingLine() : CreateRegularLine();
            line.SetViewData(ConstructUIData(rank));
            return line;
        }

        private RankLine CreatePlayerRatingLine()
        {
            return Object.Instantiate(_selfRankLinePrefab, _linesParent);
        }

        private RankLine CreateRegularLine()
        {
            return Object.Instantiate(_rankLinePrefab, _linesParent);
        }

        private bool IsThisPlayerRank(SingleRank singleRank)
        {
            return singleRank == _rankData.FindMyRank(_clickDataService.GetAllClickCount());
        }

        private RankLineUIData ConstructUIData(SingleRank singleRank)
        {
            return new RankLineUIData
            {
                rank = _rankData.TranslateToName(singleRank),
                points = singleRank.rankPoints.ToString("NO"),
            };
        }
    }
}