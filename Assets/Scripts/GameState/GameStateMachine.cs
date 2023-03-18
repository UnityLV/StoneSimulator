using System.Collections;
using Health.Interfaces;
using LocationGameObjects.Interfaces;
using Network.Interfaces;
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
        private IStoneClickEvents _stoneClickEvents;
        private INetworkCallbacks _networkCallbacks;

        [Inject]
        private void Construct(
            ILocationSpawnerService locationSpawnerService,
            IStoneSpawnerService stoneSpawnerService,
            IGetLocationCountService getLocationCountService,
            IStoneClickEvents stoneClickEvents,
            IHealthBarUIService healthBarUIService,
            IStoneAnimatorEventsInvoke stoneAnimatorEventsInvoke,
            INetworkCallbacks networkCallbacks
            )
        {
            _locationSpawner = locationSpawnerService;
            _stoneSpawner = stoneSpawnerService;
            _getLocationCountService = getLocationCountService;
            _healthBarUIService = healthBarUIService;
            _stoneAnimatorEventsInvoke = stoneAnimatorEventsInvoke;
            //_networkCallbacks = networkCallbacks;
            
            _stoneClickEvents= stoneClickEvents;
            _stoneClickEvents.OnStoneClick += OnStoneClick;
            
            _networkCallbacks = networkCallbacks;
            _networkCallbacks.OnStoneClickNetwork += OnStoneCallbackNetwork;
            _stoneClickEvents.OnStoneClick += _networkCallbacks.AddStoneClickOnServer;
        }

        #endregion

        private int _currentLocation;
        private int _currentStone;


        private int _currentHealth;
        private const int HP_PER_LVL = 4;

        private int _countClickAfterCallback = 0;

        private void Start()
        {
            _locationSpawner.SpawnLocationObjects(_currentLocation);
            _stoneSpawner.SpawnStoneObject(_currentLocation, _currentStone);
            _currentHealth = HP_PER_LVL * (_currentStone + 1);
            _healthBarUIService.UpdateHealthBarState(_currentHealth, _currentHealth);
        }

        private void OnDestroy()
        {
            _stoneClickEvents.OnStoneClick -= OnStoneClick;
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

        private void OnStoneClick()
        {
            _currentHealth -= 1;
            if (_currentHealth <= 0)
            {
                _stoneAnimatorEventsInvoke.OnStoneDestroyPlayInvoke();
                NextStone();
            }

            _healthBarUIService.UpdateHealthBarState(_currentHealth, (_currentStone + 1) * HP_PER_LVL);
            _stoneAnimatorEventsInvoke.OnStoneClickPlayInvoke();
            
            _countClickAfterCallback += 1;
        }
        
        private void OnStoneClickNetwork()
        {
            _currentHealth -= 1;
            if (_currentHealth <= 0)
            {
                _stoneAnimatorEventsInvoke.OnStoneDestroyPlayInvoke();
                NextStone();
            }

            _healthBarUIService.UpdateHealthBarState(_currentHealth, (_currentStone + 1) * HP_PER_LVL);

            _stoneAnimatorEventsInvoke.OnStoneClickPlayInvoke();
        }

        private void OnStoneCallbackNetwork(int count)
        {
            int clearCount = count - _countClickAfterCallback;
            Debug.Log($"Get server callback. All count {count}. Clear count {clearCount}");
            if (clearCount != 0) StartCoroutine(IStoneClickProcess(clearCount));
            _countClickAfterCallback = 0;
        }
        
        private IEnumerator IStoneClickProcess(int count)
        {
            float delta = _networkCallbacks.GetServerCallbackTimer()/count;
            for (int i = 0; i < count; i++)
            {
                OnStoneClickNetwork();
                yield return new WaitForSecondsRealtime(delta);
            }
        }
    }
}