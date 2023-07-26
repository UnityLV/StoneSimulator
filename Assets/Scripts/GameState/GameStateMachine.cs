using System;
using System.Collections;
using System.Collections.Generic;
using GameState.Interfaces;
using Health.Interfaces;
using LocationGameObjects.Interfaces;
using Mirror;
using PlayerData.Interfaces;
using SaveSystem;
using Stone.Interfaces;
using UnityEngine;
using Zenject;

namespace GameState
{
    public class GameStateMachine : NetworkBehaviour, IHealthService, IGameStateCallbacks, ICurrentLocationIDService,
        IGameStateService, IHealthDebugging, IChageLocationDebugging
    {
        #region Dependency

        private ILocationSpawnerService _locationSpawner;
        private IStoneSpawnerService _stoneSpawner;
        private IGetLocationCountService _getLocationCountService;
        private IHealthBarUIService _healthBarUIService;
        private IStoneAnimatorEventsInvoke _stoneAnimatorEventsInvoke;
        private IStoneClickEvents _stoneClickEvents;
        private IClickDataService _clickDataService;

        [Inject]
        private void Construct(
            ILocationSpawnerService locationSpawnerService,
            IStoneSpawnerService stoneSpawnerService,
            IGetLocationCountService getLocationCountService,
            IStoneClickEvents stoneClickEvents,
            IHealthBarUIService healthBarUIService,
            IStoneAnimatorEventsInvoke stoneAnimatorEventsInvoke,
            IClickDataService clickDataService
        )
        {
            _locationSpawner = locationSpawnerService;
            _stoneSpawner = stoneSpawnerService;
            _getLocationCountService = getLocationCountService;
            _healthBarUIService = healthBarUIService;
            _stoneAnimatorEventsInvoke = stoneAnimatorEventsInvoke;
            //_networkCallbacks = networkCallbacks;
            _clickDataService = clickDataService;

            _stoneClickEvents = stoneClickEvents;
            _stoneClickEvents.OnStoneClick += AddCLickOnServer;
        }

        #endregion

        private int _currentLocation;
        private int _currentStone;

        private static StoneData _stoneData;
        private BinarySaveSystem _stoneSaveSystem;
        private BinarySaveSystem _healthSaveSystem;

        private int _currentHealth;

        [SyncVar]
        private readonly int _hpPerLvl = 4;

        private int _countClickAfterCallback;
        private int _countClickAllCallback;

        private float _serverCallBackTime = 1f;

        private NetworkIdentity _networkIdentity;

        private const string STONE_DATA_PATCH = "StoneData";
        private const string HEALTH_DATA_PATCH = "HealthData";

        private bool _isInGame = false;

        private int _locationToLoad;

        private readonly SyncList<List<int>> _hpList = new();
        private HealthData _healthData;

        public event Action OnHealthChanged;
        public event Action OnLocationChanged;

        public void TryStartGame()
        {
            if (isServer) return;
            CmdStartGameOnClient(_networkIdentity.connectionToClient);
        }

        [Command(requiresAuthority = false)]
        private void CmdStartGameOnClient(NetworkConnectionToClient client)
        {
            StartGameTarget(client);
        }

        [Command(requiresAuthority = false)]
        private void CmdLoadGameOnClient(NetworkConnectionToClient client)
        {
            LoadGameTarget(client, _currentLocation, _currentStone, _currentHealth);
        }

        [TargetRpc]
        private void StartGameTarget(NetworkConnectionToClient target)
        {
            _isInGame = true;
            _locationSpawner.SpawnLocationObjects(_currentLocation);
            _stoneSpawner.SpawnStoneObject(_currentLocation, _currentStone);
            _healthBarUIService.UpdateHealthBarState(_currentHealth, GetHealth(_currentLocation,_currentStone));
        }   
       
        [ClientRpc]
        private void AllStartGameTarget()
        {
            _isInGame = true;
            _locationSpawner.SpawnLocationObjects(_currentLocation);
            _stoneSpawner.SpawnStoneObject(_currentLocation, _currentStone);
            _healthBarUIService.UpdateHealthBarState(_currentHealth, GetHealth(_currentLocation,_currentStone));
        }

        [TargetRpc]
        private void LoadGameTarget(NetworkConnectionToClient target, int currentLocation, int currentStone,
            int currentHealth)
        {
            _currentLocation = currentLocation;
            _currentStone = currentStone;
            _currentHealth = currentHealth;
            OnHealthChanged?.Invoke();
            OnLocationChanged?.Invoke();
        }

        [ClientRpc]
        private void AllLoadGameTarget(int currentLocation, int currentStone,
            int currentHealth)
        {
            Debug.Log(currentLocation);
            _currentLocation = currentLocation;
            _currentStone = currentStone;
            _currentHealth = currentHealth;
            OnHealthChanged?.Invoke();
            OnLocationChanged?.Invoke();
        }  
        
        public void TryWatchLocation(int id)
        {
            Debug.Log(id);
            if (isServer) return;
            CmdWatchLocationOnClient(_networkIdentity.connectionToClient);
            _locationToLoad = id;
        }

        [Command(requiresAuthority = false)]
        private void CmdWatchLocationOnClient(NetworkConnectionToClient client)
        {
            WatchLocationTarget(client);
        }

        [TargetRpc]
        private void WatchLocationTarget(NetworkConnectionToClient target)
        {
            _locationSpawner.SpawnLocationObjects(_locationToLoad);
            _isInGame = false;
            _stoneSpawner.DestroyStoneObject(true);
        }
        
        public override void OnStartServer()
        {
            _networkIdentity = GetComponent<NetworkIdentity>();
            LoadData();
            LoadHealth();
            _locationSpawner.SpawnLocationObjects(_currentLocation);
            _stoneSpawner.SpawnStoneObject(_currentLocation, _currentStone);
            _healthBarUIService.UpdateHealthBarState(_currentHealth, GetHealth(_currentLocation,_currentStone));
            StartCoroutine(IStoneServerCallbackProcess());
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            _networkIdentity = GetComponent<NetworkIdentity>();
            CmdLoadGameOnClient(_networkIdentity.connectionToClient);
        }

        private void NextStone()
        {
            if (!_isInGame && !isServer) return;

            _stoneAnimatorEventsInvoke.OnStoneDestroyPlayInvoke();
            if (_currentStone < _getLocationCountService.GetStoneCount(_currentLocation) - 1)
            {
                _currentStone += 1;
                Debug.Log("NextStone");
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
                OnLocationChanged?.Invoke();
            }

            _currentHealth = GetHealth(_currentLocation,_currentStone);
            _healthBarUIService.UpdateHealthBarState(_currentHealth, _currentHealth);
            OnHealthChanged?.Invoke();
        }

        private void OnStoneClick()
        {
            TakeDamage();

            _healthBarUIService.UpdateHealthBarState(_currentHealth, GetHealth(_currentLocation,_currentStone));
            _clickDataService.AddClicks();
            _stoneAnimatorEventsInvoke.OnStoneClickPlayInvoke();

            _countClickAfterCallback += 1;
        }

        private void OnStoneClickNetwork()
        {
            TakeDamage();
            _healthBarUIService.UpdateHealthBarState(_currentHealth, GetHealth(_currentLocation,_currentStone));

            _stoneAnimatorEventsInvoke.OnStoneClickPlayInvoke();
        }

        private void TakeDamage()
        {
            Debug.Log("Current h " + _currentHealth);
            _currentHealth = Mathf.Max(0, _currentHealth - 1);
            OnHealthChanged?.Invoke();
            if (_currentHealth == 0) NextStone();
        }

        [ClientRpc]
        private void OnStoneCallbackNetwork(int count)
        {
            int clearCount = count - _countClickAfterCallback;
            //Debug.Log($"Get server callback. All count {count}. Clear count {clearCount}");
            StartCoroutine(IStoneMultiClickSimulate(clearCount));
            _countClickAfterCallback = 0;
        }

        private void AddCLickOnServer()
        {
            Debug.Log("Add click on Server");
            CmdAddClickToServer(netIdentity.connectionToClient);
        }

        private void Test()
        {
            Debug.Log("Current h " + _currentHealth);
            _currentHealth = Mathf.Max(0, _currentHealth - 1);
            OnHealthChanged?.Invoke();
            if (_currentHealth == 0) NextStone();
        }

        [Command(requiresAuthority = false)]
        private void CmdAddClickToServer(NetworkConnectionToClient target)
        {
            _countClickAllCallback += 1;
            TakeDamage();
            //Test();
            Debug.Log("Take dmg on server");
            TargetCallbackOnClick(target);
        }

        [TargetRpc]
        private void TargetCallbackOnClick(NetworkConnectionToClient target)
        {
            OnStoneClick();
        }

        private IEnumerator IStoneServerCallbackProcess()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(_serverCallBackTime);
                OnStoneCallbackNetwork(_countClickAllCallback);
                _countClickAllCallback = 0;
                SaveData();
            }
        }

        private IEnumerator IStoneMultiClickSimulate(int count)
        {
            float delta = _serverCallBackTime / count;
            for (int i = 0; i < count; i++)
            {
                OnStoneClickNetwork();
                yield return new WaitForSecondsRealtime(delta);
            }
        }

        private void SaveData()
        {
            _stoneData.CurrentLocation = _currentLocation;
            _stoneData.CurrentStone = _currentStone;
            _stoneData.CurrentHealth = _currentHealth;
            //Debug.Log(stoneData.CurrentHealth);
            _stoneSaveSystem.Save(_stoneData);
        }

        private void LoadData()
        {
            _stoneSaveSystem ??= new BinarySaveSystem(STONE_DATA_PATCH);
            _stoneData = (StoneData) _stoneSaveSystem.Load();
            if (_stoneData == null)
            {
                _stoneData = new StoneData();
                _stoneSaveSystem.Save(_stoneData);
            }

            _currentLocation = _stoneData.CurrentLocation;
            _currentStone = _stoneData.CurrentStone;
            _currentHealth = (_stoneData.CurrentHealth != 0) ? _stoneData.CurrentHealth : _hpPerLvl * (_currentStone + 1);
        }

        private void LoadHealth()
        {
            _healthSaveSystem ??= new BinarySaveSystem(HEALTH_DATA_PATCH);
            _healthData = (HealthData) _healthSaveSystem.Load();
            if (_healthData == null)
            {
                _healthData = new HealthData();
                for (int i = 0; i < _getLocationCountService.GetLocationsCount(); i++)
                {
                    _healthData.Healths.Add(new ());
                    for (int j = 0; j < _getLocationCountService.GetStoneCount(i); j++)
                        _healthData.Healths[i].Array.Add(GetHealthLvl(j));
                }
                _healthSaveSystem.Save(_healthData);
            }
            _hpList.Clear();
            
            for (int i = 0; i < _healthData.Healths.Count; i++)
            {
                _hpList.Add(_healthData.Healths[i].Array);
            }
        }

        private void SaveHealth()
        {
            _healthSaveSystem = new BinarySaveSystem(HEALTH_DATA_PATCH);
            _healthSaveSystem.Save(_healthData);
        }

        public int GetCurrentStoneHealth()
        {
            return _currentHealth;
        }
        
        public int GetCurrentLocationHealth()
        {
            int result = _currentHealth;
            for (int i = _currentStone+1; i < _hpList[_currentLocation].Count; i++)
            {
                result += _hpList[_currentLocation][i];
            }

            return result;
        }

        public int GetMaxLocationHealth()
        {
            int result = 0;
            for (int i = 0; i < _hpList[_currentLocation].Count; i++)
            {
                result += GetHealth(_currentLocation, i);
            }

            return result;
        }

        public int GetCurrentLocationId()
        {
            return _currentLocation;
        }

        public int GetCurrentStoneId()
        {
            return _currentStone;
        }

        public int GetHealthPerLvl()
        {
            return _hpPerLvl;
        }
        
        public int GetHealth(int location, int lvl)
        {
            return _hpList[location][lvl];
        }

        public void SetHealth(int location, int lvl, int value)
        {
            CmdSaveHealth(location,lvl,value,_networkIdentity.connectionToClient);
        }

        public void SetCurrentHealth(int value)
        {
            CmdSaveCurrentHealth(value);
        }

        public int GetHealthLvl(int lvl)
        {
            return (lvl + 1) * _hpPerLvl;
        }
        
        [Command(requiresAuthority = false)]
        private void CmdSaveHealth(int location, int lvl, int value,NetworkConnectionToClient target=null)
        {
            _hpList[location][lvl] = value;
            Debug.Log(_hpList[location][lvl]);
            _healthData.Healths[location].Array[lvl] = value;
            SaveHealth();
            RpcSetHealth(location,lvl,value);
        }
        
        [ClientRpc] 
        private void RpcSetHealth(int location, int lvl, int value)
        {
            _hpList[location][lvl] = value;
        }
        
        [Command(requiresAuthority = false)]
        private void CmdSaveCurrentHealth(int value,NetworkConnectionToClient target=null)
        {
            _currentHealth = value;
            _countClickAllCallback = 0;
            RpcSetCurrentHealth(value);
        }
        
        [ClientRpc] 
        private void RpcSetCurrentHealth(int value)
        {
            _currentHealth = value;
            _countClickAfterCallback = 0;
        }

        public void ChangeLocation(int location, int stone)
        {
            CmdChangeLocation(location, stone, _networkIdentity.connectionToClient);
        }

        [Command(requiresAuthority = false)]
        private void CmdChangeLocation(int location, int stone,NetworkConnectionToClient target=null)
        {
            _currentLocation = location;
            _currentStone = stone;
            _currentHealth = GetHealth(location, stone);
            SaveData();
            AllLoadGameTarget(_currentLocation,_currentStone,_currentHealth);
            AllStartGameTarget();
        }
    }
}