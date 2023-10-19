using System;
using ChatDB;
using InGameUI.Interfaces;
using MongoDBCustom;
using Stone.Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace LocationGameObjects
{
    public interface IClickOnStoneEffects
    {
        void SetPoints(StoneClickEffectPoints stoneClickEffectPoints);
        void SetDataForChoiceEffect(int location, int stoneLvl);
    }

    public class ClickOnStoneEffects : IClickOnStoneEffects, IDisposable
    {
        private IStoneClickEvents _stoneClickEvents;
        private IGetStoneClickEffectService _getStoneClickEffectService;

        private StoneClickEffectPoints _stoneClickEffectPoints;
        private GameObject _effectPrefab;

        public ClickOnStoneEffects(IStoneClickEvents stoneClickEvents,
            IGetStoneClickEffectService getStoneClickEffectService)
        {
            _stoneClickEvents = stoneClickEvents;
            _getStoneClickEffectService = getStoneClickEffectService;
            _stoneClickEvents.OnStoneClick += OnStoneClick;
        }

        private void OnStoneClick()
        {
            var effect = GameObject.Instantiate(_effectPrefab, GetPointToEffect(), Quaternion.identity);
            GameObject.Destroy(effect, 1f);
        }

        public void SetPoints(StoneClickEffectPoints stoneClickEffectPoints)
        {
            
            _stoneClickEffectPoints = stoneClickEffectPoints;
        }

        public void SetDataForChoiceEffect(int location, int stoneLvl)
        {
            _effectPrefab = _getStoneClickEffectService.GetClickEffect(location, stoneLvl);
        }

        private Vector3 GetPointToEffect()
        {
            if (_stoneClickEffectPoints == null)
            {
                Debug.LogWarning($"On Stone must put component {nameof(StoneClickEffectPoints)} ");
                return Vector3.forward;
            }
            return _stoneClickEffectPoints.GetPointForEffectOnStone();
        }

        public void Dispose()
        {
            _stoneClickEvents.OnStoneClick -= OnStoneClick;
        }
    }
}