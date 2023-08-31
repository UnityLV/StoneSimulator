using System;
using FirebaseCustom;
using MongoDB.Bson;
using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace MongoDBCustom
{
    public class ReferrerClicks : IDBReferrerClicksSaver, IDisposable
    {
        private IStoneClickEvents _stoneClickEvents;
        private IDBCommands _idbCommands;

        private float _percentToAdd => RemoteConfigSetter.PlayerConfig.PercentToAddToReferrer;
        private int _clickCount = 0;

        [Inject]
        private void Construct(IStoneClickEvents stoneClickEvents, IDBCommands idbCommands)
        {
            _stoneClickEvents = stoneClickEvents;
            _idbCommands = idbCommands;
            _stoneClickEvents.OnStoneClick += OnStoneClick;
        }

        private void OnStoneClick()
        {
            _clickCount++;
        }

        public void Save()
        {
            int clicksToAdd = Mathf.RoundToInt(_clickCount * (_percentToAdd / 100f));
            if (clicksToAdd == 0)
            {
                return;
            }

            _idbCommands.AddPlayerClickToGiveReferrer(clicksToAdd).ContinueWith(
                (task) => _clickCount = 0);
        }

        public void Dispose()
        {
            _stoneClickEvents.OnStoneClick -= OnStoneClick;
        }
    }
}