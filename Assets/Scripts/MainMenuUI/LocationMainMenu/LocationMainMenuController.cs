using System;
using System.Collections.Generic;
using GameState.Interfaces;
using LocationGameObjects.Interfaces;
using MainMenuUI.LocationMainMenu.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MainMenuUI.LocationMainMenu
{
    public class LocationMainMenuController : MonoBehaviour
    {
        [SerializeField]
        private Transform _locationParent;

        [SerializeField]
        private Image _mainBG;
        
        private List<LocationMainMenuObject> _locationMainMenuList;
        private LocationMainMenuObject _currentLocationObject;

        private Action _inProgressAction;
        private Action<int> _completeAction;

        #region Dependency

        private ILocationFactoryService _locationFactoryService;
        private IGetLocationCountService _getLocationCountService;
        private IGetLocationSpritesService _getLocationSpritesService;
        private ICurrentLocationIDService _currentLocationIDService;
        private IGameStateCallbacks _gameStateCallbacks;
        private IHealthService _healthService;

        [Inject]
        private void Construct(ILocationFactoryService locationFactoryService,
            IGetLocationCountService getLocationCountService,
            IGetLocationSpritesService getLocationSpritesService,
            ICurrentLocationIDService currentLocationIDService,
            IHealthService healthService, IGameStateCallbacks gameStateCallbacks)
        {
            _locationFactoryService = locationFactoryService;
            _getLocationCountService = getLocationCountService;
            _getLocationSpritesService = getLocationSpritesService;
            _currentLocationIDService = currentLocationIDService;
            _healthService = healthService;
            _gameStateCallbacks = gameStateCallbacks;
        }

        #endregion

        private void Start()
        {
            _locationMainMenuList = _locationFactoryService.CreateLocationObjects(_getLocationCountService.GetLocationsCount());
        }

        public void SubscribeCallbacks()
        {
            _gameStateCallbacks.OnLocationChanged += UpdateLocationUI;
            _gameStateCallbacks.OnHealthChanged += UpdateCurrentLocation;
        }

        public void UnsubscribeCallbacks()
        {
            
            _gameStateCallbacks.OnLocationChanged -= UpdateLocationUI;
            _gameStateCallbacks.OnHealthChanged -= UpdateCurrentLocation;
        }

        public void UpdateCurrentLocation()
        {
            float curHp = _healthService.GetCurrentLocationHealth();
            float maxHp = _healthService.GetMaxLocationHealth();
            int locationId = _currentLocationIDService.GetCurrentLocationId();
            _locationMainMenuList[locationId]
                .SetInProgressState(locationId,(maxHp - curHp) / maxHp);
            ChangeCurrentObject(_locationMainMenuList[_currentLocationIDService.GetCurrentLocationId()]);
        }

        public void UpdateLocationUI()
        {
            for (var i = 0; i < _locationMainMenuList.Count; i++)
            {
                var obj = _locationMainMenuList[i];
                obj.transform.SetParent(_locationParent);
                obj.transform.localScale= Vector3.one;
                obj.SetLocationController(this);
                obj.SetLocationSprite(_getLocationSpritesService.GetAvatarLocationSprite(i));
                if (i < _currentLocationIDService.GetCurrentLocationId()) obj.SetCompleteState(i);
                else if (i == _currentLocationIDService.GetCurrentLocationId()) UpdateCurrentLocation();
                else obj.SetLockedState(i,i-1 == _currentLocationIDService.GetCurrentLocationId());
            }
        }

        public void ChangeCurrentObject(LocationMainMenuObject locationMainMenuObject)
        {
            if (_currentLocationObject == locationMainMenuObject) return;
            if (_currentLocationObject != null) _currentLocationObject.ChangeStateChosen(false);
            _currentLocationObject = locationMainMenuObject;
            _currentLocationObject.ChangeStateChosen(true);
            _mainBG.sprite = _getLocationSpritesService.GetBGLocationSprite(_locationMainMenuList.IndexOf(_currentLocationObject));
        }

        public void OnPlayBTNClick(LocationMainMenuObject locationMainMenuObject)
        {
            int id = _locationMainMenuList.IndexOf(locationMainMenuObject);
            Debug.Log(id);
            if (id == _currentLocationIDService.GetCurrentLocationId()) _inProgressAction?.Invoke();
            else if (id <_currentLocationIDService.GetCurrentLocationId()) _completeAction?.Invoke(id);
        }
        
        public void SetOnCompleteLocationClickAction(Action<int> action)
        {
            _completeAction = action;
        }

        public void SetInProgressLocationClickAction(Action action)
        {
            _inProgressAction = action;
        }
    }
}