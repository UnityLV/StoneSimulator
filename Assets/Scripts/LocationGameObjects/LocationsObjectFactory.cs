using System.Collections.Generic;
using System.Threading.Tasks;
using LocationGameObjects.Interfaces;
using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace LocationGameObjects
{
    public class LocationsObjectFactory : ILocationSpawnerService, IStoneSpawnerService
    {
        #region Dependency

        private readonly IGetLocationGameObjectService _getLocationGameService;
        private DiContainer _diContainer;

        public LocationsObjectFactory(
            IGetLocationGameObjectService getLocationGameService,
            DiContainer diContainer,
            IStoneAnimatorEventCallback animatorEvents)
        {
            _diContainer = diContainer;
            _getLocationGameService = getLocationGameService;
            animatorEvents.OnStoneDestroyed += () => _isDestroyedCallbacks = true;
        }

        #endregion

        private List<GameObject> _currentLocationObject;
        private GameObject _currentStone;

        private bool _isDestroyedCallbacks;

        public void SpawnLocationObjects(int location)
        {
            if (_currentLocationObject != null)
                for (int i = 0; i < _currentLocationObject.Count; i++)
                    Object.Destroy(_currentLocationObject[i]);
            _currentLocationObject = new List<GameObject>();
            List<GameObject> locationPrefabs = _getLocationGameService.GetLocationsGameObject(location);
            foreach (GameObject gameObjectPrefab in locationPrefabs)
            {
                _currentLocationObject.Add(_diContainer.InstantiatePrefab(gameObjectPrefab));
            }
        }

        public async void SpawnStoneObject(int location, int stoneLvl)
        {
            if (_currentStone != null)
            {
                await AwaitDestroy();
                Object.Destroy(_currentStone);
            }

            _currentStone = null;
            GameObject stonePrefab = _getLocationGameService.GetStoneGameObject(location, stoneLvl);
            _currentStone = _diContainer.InstantiatePrefab(stonePrefab);
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