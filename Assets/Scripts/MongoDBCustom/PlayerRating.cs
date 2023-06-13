using System;
using PlayerData.Interfaces;
using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace MongoDBCustom
{
    public class PlayerRating : MonoBehaviour
    {
        private IClickDataService _clickData;
        private IStoneClickEvents _clickEvent;
        private IDBRatingSaver _ratingSaver;

        private const int MinTimeBetweenUpdatesInSeconds = 10;

        private DateTime _lastUpdateTimestamp;

        [Inject]
        private void Construct(
            IStoneClickEvents stoneClickEvents,
            IClickDataService clickDataService, 
            IDBRatingSaver dbRatingSaver
        )
        {
            _clickData = clickDataService;
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