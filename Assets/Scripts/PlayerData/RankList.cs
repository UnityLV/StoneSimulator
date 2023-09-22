using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using InGameUI;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

namespace PlayerData
{
    public class RankList : MonoBehaviour
    {
        [SerializeField] private RankListLineFactory _lineFactory;
        [SerializeField] private RankData _rankData;
        private readonly List<RankLine> _lines = new();

        [Inject]
        private void Construct(IClickDataService clickDataService)
        {
            _lineFactory.SetDep(clickDataService);
        }

        private void OnEnable()
        {
            LocalizationManager.OnLocalizeEvent += UpdateText;
        }

        private void Start()
        {
            DrawRankList();
        }

        private void OnDisable()
        {
            LocalizationManager.OnLocalizeEvent -= UpdateText;
        }

        private void UpdateText()
        {
            DrawRankList();
        }

        private void DrawRankList()
        {
            ClearLines();
            var sortedRanks = _rankData.GetRanks().OrderBy(d => d.rankPoints).ToArray();

            foreach (SingleRank rank in sortedRanks)
            {
                RankLine line = _lineFactory.CreateLine(rank);
                _lines.Add(line);
            }
        }

        private void ClearLines()
        {
            if (_lines == null) return;
            foreach (var line in _lines)
            {
                Destroy(line.gameObject);
            }
            _lines.Clear();
        }
    }
}