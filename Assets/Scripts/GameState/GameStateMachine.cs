using Health.Interfaces;
using LocationGameObjects.Interfaces;
using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace GameState
{
    public class GameStateMachine : MonoBehaviour
    {
        #region Dependency

        private ILocationSpawnerService _locationSpawner;
        private IStoneSpawnerService _stoneSpawner;
        private IGetLocationCountService _getLocationCountService;
        private IHealthBarUIService _healthBarUIService;
        private IStoneAnimatorEventsInvoke _stoneAnimatorEventsInvoke;

        [Inject]
        private void Construct(
            ILocationSpawnerService locationSpawnerService,
            IStoneSpawnerService stoneSpawnerService,
            IGetLocationCountService getLocationCountService,
            IStoneClickEvents stoneClickEvents,
            IHealthBarUIService healthBarUIService,
            IStoneAnimatorEventsInvoke stoneAnimatorEventsInvoke)
        {
            _locationSpawner = locationSpawnerService;
            _stoneSpawner = stoneSpawnerService;
            _getLocationCountService = getLocationCountService;
            stoneClickEvents.OnStoneClick += OnStoneClick;
            _healthBarUIService = healthBarUIService;
            _stoneAnimatorEventsInvoke = stoneAnimatorEventsInvoke;
        }

        #endregion

        private int _currentLocation;
        private int _currentStone;


        private int _currentHealth;
        private const int HP_PER_LVL = 4;

        private void Start()
        {
            _locationSpawner.SpawnLocationObjects(_currentLocation);
            _stoneSpawner.SpawnStoneObject(_currentLocation, _currentStone);
            _currentHealth = HP_PER_LVL * (_currentStone + 1);
            _healthBarUIService.UpdateHealthBarState(_currentHealth, _currentHealth);
        }

        private void NextStone()
        {
            if (_currentStone < _getLocationCountService.GetStoneCount(_currentLocation) - 1)
            {
                _currentStone += 1;
                _stoneSpawner.SpawnStoneObject(_currentLocation, _currentStone);
            }
            else
            {
                _currentStone = 0;
                _currentLocation = _currentLocation < _getLocationCountService.GetLocationsCount() - 1
                    ? _currentLocation + 1
                    : 0;
                _locationSpawner.SpawnLocationObjects(_currentLocation);
                _stoneSpawner.SpawnStoneObject(_currentLocation, _currentStone);
            }

            _currentHealth = HP_PER_LVL * (_currentStone + 1);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) NextStone();
        }

        private void OnStoneClick()
        {
            _currentHealth -= 1;
            if (_currentHealth <= 0)
            {
                _stoneAnimatorEventsInvoke.OnStoneDestroyPlayInvoke();
                NextStone();
            }

            _healthBarUIService.UpdateHealthBarState(_currentHealth, (_currentStone + 1) * HP_PER_LVL);
        }
    }
}