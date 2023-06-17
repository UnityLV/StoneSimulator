using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace InGameUI
{
    public class RatingListUI : MonoBehaviour
    {
        private RatingSingleLine[] _lines;

        private void Awake()
        {
            _lines = GetComponentsInChildren<RatingSingleLine>();
        }

        public void SetPlayersData(IEnumerable<RatingPlayerData> playersData )
        {
            RatingPlayerData[] sortedData = playersData.OrderBy(d => d.RatingNumber).ToArray();
            
            for (int i = 0; i < sortedData.Length; i++)
            {
                _lines[i].SetData(sortedData[i]);
            }
        }
        
    }
}