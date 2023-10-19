using System;
using Stone.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LocationGameObjects
{
    public interface IAbilityClickEffect
    {
        void SetEffectPoint(Transform point);
    }

    public class AbilityClickEffect : IAbilityClickEffect, IDisposable
    {
        private IAbilityClickEvents _abilityClickEvents;
        private IGetAbilityEffectService _abilityEffectService;
        private Transform _effectPoint;

        public AbilityClickEffect(IAbilityClickEvents abilityClickEvents, IGetAbilityEffectService abilityEffectService)
        {
            _abilityClickEvents = abilityClickEvents;
            _abilityEffectService = abilityEffectService;
            _abilityClickEvents.OnAbilityClick += OnAbilityClick;
        }

        private void OnAbilityClick(int clicks)
        {
            SpawnGroundEffect(clicks);
            SpawnTopEffect(clicks);
        }

        private void SpawnTopEffect(int clicks)
        {
            GameObject effectPrefab = _abilityEffectService.GetTopEffect(clicks);
            GameObject effect = Object.Instantiate(effectPrefab, Vector3.zero, Quaternion.identity);
            Object.Destroy(effect, 7f);
        }

        private void SpawnGroundEffect(int clicks)
        {
            GameObject effectPrefab = _abilityEffectService.GetGroundEffect(clicks);
            GameObject effect = Object.Instantiate(effectPrefab, GetSpawnPoint(), Quaternion.identity);
            Object.Destroy(effect, 5f);
        }

        private Vector3 GetSpawnPoint()
        {
            return _effectPoint != null ? _effectPoint.position : Vector3.down;
        }

        public void SetEffectPoint(Transform point)
        {
            _effectPoint = point;
        }

        public void Dispose()
        {
            _abilityClickEvents.OnAbilityClick -= OnAbilityClick;
        }
    }
}