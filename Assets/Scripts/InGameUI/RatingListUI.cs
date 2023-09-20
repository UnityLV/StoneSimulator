using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MongoDBCustom;
using PlayerData;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

namespace InGameUI
{
    public class RatingListUI : MonoBehaviour
    {
        [SerializeField] private Transform _lineParent;
        [SerializeField] private RatingSingleLine _linePrefab;

        protected INicknameDataService NicknameData;
        protected IClickDataService ClickData;

        private RatingSingleLine[] _lines;

        [Inject]
        private void Construct(INicknameDataService InicknameDataService, IClickDataService clickDataService)
        {
            NicknameData = InicknameDataService;
            ClickData = clickDataService;

            CreateLines();
        }

        private void CreateLines()
        {
            _lines = new RatingSingleLine [100];
            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i] = Instantiate(_linePrefab, _lineParent);
            }
        }


        public void SetData(IEnumerable<RatingPlayerData> playersData)
        {
            SetPlayersData(playersData);
            OnSetData();
        }


        private void SetPlayersData(IEnumerable<RatingPlayerData> playersData)
        {
            RatingPlayerData[] sortedData = playersData.OrderBy(d => d.RatingNumber).ToArray();

            for (int i = 0; i < sortedData.Length; i++)
            {
                RatingSingleLine line = _lines[i];
                line.SetData(sortedData[i]);
                line.transform.SetSiblingIndex(i);
                SetPlayerRatingNumber(sortedData, i);
            }
        }


        private void SetPlayerRatingNumber(RatingPlayerData[] sortedData, int i)
        {
            if (sortedData[i].Name == NicknameData.GetNickname())
            {
                OnFindPlayerInRating(i + 1);
            }
        }

        protected virtual void OnFindPlayerInRating(int rating)
        {
        }

        protected virtual void OnSetData()
        {
        }
    }
}