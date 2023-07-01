using System;
using System.Linq;
using UnityEngine;

namespace PlayerData
{
    [CreateAssetMenu(fileName = "RankData", menuName = "Custom/Rank Data", order = 1)]
    public class RankData : ScriptableObject
    {
        [SerializeField] private SingleRank[] _ranks;

        public SingleRank GetRankByClicks(int points)
        {
            _ranks = _ranks.OrderBy(rank => rank.rankPoints).ToArray();
            
            for (int i = _ranks.Length - 1; i >= 0; i--)
            {
                if (points >= _ranks[i].rankPoints)
                {
                    return _ranks[i];
                }
            }

            return _ranks[0];
        }
    }
}