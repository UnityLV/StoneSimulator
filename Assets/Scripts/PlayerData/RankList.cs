using System.Collections.Generic;
using System.Linq;
using InGameUI;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

namespace PlayerData
{
    public class RankList : MonoBehaviour
    {
        [SerializeField] private RankListLineFactory _lineFactory;
        private List<RankLine> _lines = new();

        [Inject]
        private void Construct(IClickDataService clickDataService)
        {
            _lineFactory.SetDep(clickDataService);
        }

        private void ClearLines()
        {
            if (_lines == null) return;
            foreach (var line in _lines)
            {
                Destroy(line.gameObject);
            }
        }

        private void SetData(IEnumerable<SingleRank> rankLines)
        {
            ClearLines();
            var sortedData = rankLines.OrderBy(d => d.rankPoints).ToArray();

            foreach (SingleRank rank in sortedData)
            {
                RankLine rankLine = _lineFactory.CreateLine(rank);
            }
        }
    }
}