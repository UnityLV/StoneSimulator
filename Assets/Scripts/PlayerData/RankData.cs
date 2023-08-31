using System;
using System.Linq;
using I2.Loc;
using NaughtyAttributes;
using UnityEngine;

namespace PlayerData
{
    [CreateAssetMenu(fileName = "RankData", menuName = "Custom/Rank Data", order = 1)]
    public class RankData : ScriptableObject
    {
        [SerializeField] private SingleRank[] _ranks;

        public string JSON;

        public string GetRankByClicks(int points)
        {
            _ranks = _ranks.OrderBy(rank => rank.rankPoints).ToArray();

            SingleRank selectedRank = _ranks[0];

            for (int i = _ranks.Length - 1; i >= 0; i--)
            {
                if (points >= _ranks[i].rankPoints)
                {
                    selectedRank = _ranks[i];
                    break;
                }
            }

            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                return selectedRank.rankNameRu;
            }

            return selectedRank.rankNameEn;
        }

        [Button()]
        public void ToJson()
        {
            Debug.Log(JSON = JsonUtility.ToJson(new RanksWrapper { ranks = _ranks }, true));
        }

        public void FromJson(string json)
        {
            var data = JsonUtility.FromJson<RanksWrapper>(json);
            _ranks = data.ranks;
        }

        [Serializable]
        private class RanksWrapper
        {
            public SingleRank[] ranks;
        }
    }
}