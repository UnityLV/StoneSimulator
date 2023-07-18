using System;
using PlayerData.Interfaces;
using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace MongoDBCustom
{
    public class PlayerRatingInDBSaver : MonoBehaviour
    {
        private IStoneClickEvents _clickEvent;
        private IDBAllClickSaver _ratingSaver;

        private const int MinTimeBetweenUpdatesInSeconds = 100;

        private DateTime _lastUpdateTimestamp;

        [Inject]
        private void Construct(
            IStoneClickEvents stoneClickEvents,
            IDBAllClickSaver idbAllClickSaver
        )
        {
            _clickEvent = stoneClickEvents;
            _ratingSaver = idbAllClickSaver;
            _clickEvent.OnStoneClick += HandleStoneClick;
        }

        private void OnDestroy()
        {
            _clickEvent.OnStoneClick -= HandleStoneClick;
        }

        private void HandleStoneClick(int _)
        {
            if (ShouldUpdateRating())
            {
                UpdateRating();
            }
        }

        private bool ShouldUpdateRating()
        {
            return (DateTime.Now - _lastUpdateTimestamp).TotalSeconds >= MinTimeBetweenUpdatesInSeconds;
        }

        private void UpdateRating()
        {
            _ratingSaver?.Save();
            _lastUpdateTimestamp = DateTime.Now;
        }
    }
}