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
        private IDBRatingSaver _ratingSaver;

        private const int MinTimeBetweenUpdatesInSeconds = 100;

        private DateTime _lastUpdateTimestamp;

        [Inject]
        private void Construct(
            IStoneClickEvents stoneClickEvents,
            IDBRatingSaver dbRatingSaver
        )
        {
            _clickEvent = stoneClickEvents;
            _ratingSaver = dbRatingSaver;
            _clickEvent.OnStoneClick += HandleStoneClick;
        }

        private void OnDestroy()
        {
            _clickEvent.OnStoneClick -= HandleStoneClick;
        }

        private void HandleStoneClick()
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
            _ratingSaver?.SaveRating();
            _lastUpdateTimestamp = DateTime.Now;
        }
    }
}