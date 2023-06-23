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
        protected INicknameDataService NicknameData;
        
        protected IClickDataService ClickData;

        private RatingSingleLine[] _lines;
        
        [Inject]
        private void Construct(INicknameDataService InicknameDataService,IClickDataService clickDataService)
        {
            NicknameData = InicknameDataService;
            ClickData = clickDataService;
        }


        public void SetData(IEnumerable<RatingPlayerData> playersData)
        {
            SetLines();
            SetPlayersData(playersData);
            OnSetData();
        }

        private void SetPlayersData(IEnumerable<RatingPlayerData> playersData)
        {
            RatingPlayerData[] sortedData = playersData.OrderBy(d => d.RatingNumber).ToArray();

            for (int i = 0; i < sortedData.Length; i++)
            {
                _lines[i].SetData(sortedData[i]);

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

        protected virtual void OnFindPlayerInRating(int raiting)
        {
        }

        protected virtual void OnSetData()
        {
        }

        private void SetLines()
        {
            _lines = GetComponentsInChildren<RatingSingleLine>();
        }
    }
}