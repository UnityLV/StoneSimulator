using System;
using MongoDB.Bson;
using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace MongoDBCustom
{
    public class ReferrerClicks : IDBReferrerClicksSaver, IDisposable
    {
        private IStoneClickEvents _stoneClickEvents;
        private IDBValues _dbValues;
        
        private readonly float _percentToAdd = 0.1f;
        private int _clickCount = 0;

        [Inject]
        private void Construct(IStoneClickEvents stoneClickEvents,IDBValues dbValues)
        {
            _stoneClickEvents = stoneClickEvents;
            _dbValues = dbValues;
            _stoneClickEvents.OnStoneClick += OnStoneClick;
        }

        private void OnStoneClick(int _)
        {
            _clickCount++;
        }

        public void Save()
        {
            int clicksToAdd = Mathf.RoundToInt(_clickCount * _percentToAdd);
            if (clicksToAdd == 0)
            {
                return;
            }
        
            _dbValues.AddPlayerClickToGiveReferrer(clicksToAdd).ContinueWith(
                (task)=> _clickCount = 0);
        }

        public void Dispose()
        {
            _stoneClickEvents.OnStoneClick -= OnStoneClick;
        }
    }
}