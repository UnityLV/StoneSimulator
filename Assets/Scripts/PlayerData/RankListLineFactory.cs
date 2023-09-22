using System;
using System.Text;
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
                points = FormatPointsWithDots(singleRank.rankPoints)
            };
        }

        private string FormatPointsWithDots(int points)
        {
            string numberString = points.ToString();
            int length = numberString.Length;
            if (length <= 3)
            {
                return numberString; 
            }

            StringBuilder result = new StringBuilder();
            int separatorPosition = length % 3;
            if (separatorPosition == 0)
            {
                separatorPosition = 3;
            }

            for (int i = 0; i < length; i++)
            {
                if (i == separatorPosition)
                {
                    result.Append(".");
                    separatorPosition += 3;
                }
                result.Append(numberString[i]);
            }

            return result.ToString();
        }
    }
}