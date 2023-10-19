using System.Collections.Generic;
using System.Threading.Tasks;
using LocationGameObjects.Interfaces;
using Stone.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace LocationGameObjects
{
    public class LocationsObjectFactory : ILocationSpawnerService, IStoneSpawnerService
    {
        #region Dependency

        private readonly IGetLocationGameObjectService _getLocationGameService;
        private readonly IConstantLocationEffects _constantLocationEffects;
        private readonly IClickOnStoneEffects _clickOnStoneEffects;
        private readonly IAbilityClickEffect _abilityClickEffect;
        private DiContainer _diContainer;

        public LocationsObjectFactory(
            IGetLocationGameObjectService getLocationGameService,
            DiContainer diContainer,
            IStoneAnimatorEventCallback animatorEvents, IConstantLocationEffects constantLocationEffects,
            IClickOnStoneEffects clickOnStoneEffects, IAbilityClickEffect abilityClickEffect)
        {
            _diContainer = diContainer;
            _getLocationGameService = getLocationGameService;
            _constantLocationEffects = constantLocationEffects;
            _clickOnStoneEffects = clickOnStoneEffects;
            _abilityClickEffect = abilityClickEffect;
            animatorEvents.OnStoneDestroyed += () => _isDestroyedCallbacks = true;
        }

        #endregion

        private List<GameObject> _currentLocationObject;
        private GameObject _currentStone;

        private bool _isDestroyedCallbacks;

        public void SpawnLocationObjects(int location)
        {
            DestroyLocationObject();
            _currentLocationObject = new List<GameObject>();
            List<GameObject> locationPrefabs = _getLocationGameService.GetLocationsGameObject(location);
            foreach (GameObject gameObjectPrefab in locationPrefabs)
            {
                Debug.Log(gameObjectPrefab);
                _currentLocationObject.Add(_diContainer.InstantiatePrefab(gameObjectPrefab));

                if (gameObjectPrefab.TryGetComponent(out GroundEffectPoint point))
                {
                    _constantLocationEffects.SetConstantEffectPoint(point.GetEffectPoint());
                    _abilityClickEffect.SetEffectPoint(point.GetEffectPoint());
                }
            }
        }

        public void DestroyLocationObject()
        {
            if (_currentLocationObject == null)
                return;
            for (int i = 0; i < _currentLocationObject.Count; i++)
                Object.Destroy(_currentLocationObject[i]);
            _currentLocationObject = null;
        }

        public async void SpawnStoneObject(int location, int stoneLvl)
        {
            Debug.Log("Task Destroy Stone Object");
            await TaskDestroyStoneObject();

            _currentStone = null;
            GameObject stonePrefab = _getLocationGameService.GetStoneGameObject(location, stoneLvl);
            Debug.Log(stonePrefab);
            _currentStone = _diContainer.InstantiatePrefab(stonePrefab);
            InitEffects(location, stoneLvl);
        }

        private void InitEffects(int location, int stoneLvl)
        {
            _constantLocationEffects.ShowConstantEffect(location, stoneLvl);
            _clickOnStoneEffects.SetDataForChoiceEffect(location, stoneLvl);
            if (_currentStone.TryGetComponent(out StoneClickEffectPoints points) == false)
            {
                points = _currentStone.AddComponent<StoneClickEffectPoints>();
            }

            _clickOnStoneEffects.SetPoints(points);
        }

        public async void DestroyStoneObject(bool force)
        {
            Debug.Log($"Try destroy. Current stone {_currentStone}");
            if (_currentStone == null)
                return;
            if (!force)
                await AwaitDestroy();
            Object.Destroy(_currentStone);

            _currentStone = null;
        }

        public async Task TaskDestroyStoneObject()
        {
            Debug.Log($"Try destroy. Current stone{_currentStone}");

            if (_currentStone is null)
            {
                Debug.Log("_currentStone is null");
                return;
            }

            await AwaitDestroy();
            Object.Destroy(_currentStone);

            _currentStone = null;
        }

        private async Task AwaitDestroy()
        {
            while (!_isDestroyedCallbacks)
            {
                await Task.Yield();
            }

            _isDestroyedCallbacks = false;
        }
    }
}