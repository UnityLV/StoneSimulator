using System;
using System.Collections.Generic;
using System.Linq;
using PlayerData.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InGameUI
{
    [Serializable]
    public class RatingLineFactory
    {
        [SerializeField] private Transform _lineParent;
        [SerializeField] private RatingSingleLine _linePrefab;
        [SerializeField] private RatingSingleLine _thisPlayerLinePrefab;

        private INicknameDataService _NicknameDataService;

        public void SetDep(INicknameDataService nicknameDataService)
        {
            _NicknameDataService = nicknameDataService;
        }

        public RatingSingleLine[] CreateLines(IEnumerable<RatingPlayerData> playersData)
        {
            RatingPlayerData[] sortedData = playersData.OrderBy(d => d.RatingNumber).ToArray();
            RatingSingleLine[] lines = new RatingSingleLine[sortedData.Length];

            for (int i = 0; i < sortedData.Length; i++)
            {
                RatingSingleLine line = SpawnLine(sortedData[i]);
                line.SetData(sortedData[i]);
                line.transform.SetSiblingIndex(i);
                lines[i] = line;
            }

            return lines;
        }

        private RatingSingleLine SpawnLine(RatingPlayerData lineData)
        {
            if (IsThisPlayer(lineData))
            {
                return Object.Instantiate(_thisPlayerLinePrefab, _lineParent);
            }
            return Object.Instantiate(_linePrefab, _lineParent);
        }

        private bool IsThisPlayer(RatingPlayerData sortedData)
        {
            return sortedData.Name == _NicknameDataService.GetNickname();
        }
    }
}