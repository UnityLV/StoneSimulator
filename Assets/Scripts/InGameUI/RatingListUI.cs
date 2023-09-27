using System;
using System.Collections.Generic;
using PlayerData.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InGameUI
{
    public class RatingListUI : MonoBehaviour
    {
        protected INicknameDataService NicknameData;
        protected IClickDataService ClickData;

        [SerializeField] private RatingLineFactory _lineFactory;
        [SerializeField] private Scrollbar _scrollbar;
        private RatingSingleLine[] _ratingLines = Array.Empty<RatingSingleLine>();

        [Inject]
        private void Construct(INicknameDataService InicknameDataService, IClickDataService clickDataService)
        {
            NicknameData = InicknameDataService;
            ClickData = clickDataService;
            _lineFactory.SetDep(InicknameDataService);
        }

        public void SetData(IEnumerable<RatingPlayerData> playersData)
        {
            SetRating(playersData);
            OnSetData();
        }

        private void SetRating(IEnumerable<RatingPlayerData> playersData)
        {
            _scrollbar.value = 1;
            ClearLines();
            _ratingLines = _lineFactory.CreateLines(playersData);
            FindPlayerInRatingLines();
        }

        private void ClearLines()
        {
            for (int i = 0; i < _ratingLines.Length; i++)
            {
                Destroy(_ratingLines[i].gameObject);
            }
        }

        private void FindPlayerInRatingLines()
        {
            for (int i = 0; i < _ratingLines.Length; i++)
            {
                if (NicknameData.GetNickname() == _ratingLines[i].Data.Name)
                {
                    OnFindPlayerInRating(i + 1);
                }
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