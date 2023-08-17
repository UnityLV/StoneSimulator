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

        #region Fields

        private int _currentLocation;
        private int _currentStone;

        private static StoneData _stoneData;
        private BinarySaveSystem _stoneSaveSystem;
        private BinarySaveSystem _healthSaveSystem;

        [SyncVar]
        private int _currentHealth;




        [SyncVar]
        private readonly int _hpPerLvl = 10;

        private int _countDamageAfterCallback;
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

        #endregion

        #region Start

        public void TryStartGame()
        {
            if (isServer)
                return;
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
            LoadGameTarget(client, _currentLocation, _currentStone);
        }

        [TargetRpc]
        private void StartGameTarget(NetworkConnectionToClient target)
        {
            _isInGame = true;
            _locationSpawner.SpawnLocationObjects(_currentLocation);
            _stoneSpawner.SpawnStoneObject(_currentLocation, _currentStone);
            _healthBarUIService.UpdateHealthBarState(_currentHealth, GetHealth(_currentLocation, _currentStone));
        }

        [ClientRpc]
        private void AllStartGameTarget()
        {
            _isInGame = true;
            _locationSpawner.SpawnLocationObjects(_currentLocation);
            _stoneSpawner.SpawnStoneObject(_currentLocation, _currentStone);
            _healthBarUIService.UpdateHealthBarState(_currentHealth, GetHealth(_currentLocation, _currentStone));
        }

        [TargetRpc]
        private void LoadGameTarget(NetworkConnectionToClient target,
            int currentLocation,
            int currentStone)
        {
            _currentLocation = currentLocation;
            _currentStone = currentStone;
            OnHealthChanged?.Invoke();
            OnLocationChanged?.Invoke();
        }

        [ClientRpc]
        private void AllLoadGameTarget(int currentLocation,
            int currentStone)
        {
            Debug.Log(currentLocation);
            _currentLocation = currentLocation;
            _currentStone = currentStone;
            OnHealthChanged?.Invoke();
            OnLocationChanged?.Invoke();
        }

        #endregion

        public void TryWatchLocation(int id)
        {
            Debug.Log(id);
            if (isServer)
                return;
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
            _healthBarUIService.UpdateHealthBarState(_currentHealth, GetHealth(_currentLocation, _currentStone));
            StartCoroutine(IStoneServerCallbackProcess());
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            _networkIdentity = GetComponent<NetworkIdentity>();
            CmdLoadGameOnClient(_networkIdentity.connectionToClient);
        }

        [ClientRpc]
        private void OnStoneChangeOnServer()
        {
            Debug.Log("OnStoneChangeOnServer " + _currentStone);
            _stoneAnimatorEventsInvoke.OnStoneDestroyPlayInvoke();

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
                OnLocationChanged?.Invoke();
            }

            _healthBarUIService.UpdateHealthBarState(_currentHealth, _currentHealth);

            // _currentHealth = GetHealth(_currentLocation, _currentStone);
            OnHealthChanged?.Invoke();
        }

        private void NextStone()
        {
            if (!_isInGame && !isServer)
                return;

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

            _healthBarUIService.UpdateHealthBarState(_currentHealth, _currentHealth);
            _currentHealth = GetHealth(_currentLocation, _currentStone);
            OnHealthChanged?.Invoke();
            OnStoneChangeOnServer();
        }

        private void OnStoneClick(int damage) //3
        {
            TakeDamage(damage);

            _healthBarUIService.UpdateHealthBarState(_currentHealth, GetHealth(_currentLocation, _currentStone));
            _clickDataService.AddClicks();
            _stoneAnimatorEventsInvoke.OnStoneClickPlayInvoke();

            _countDamageAfterCallback += damage;

            if (_currentHealth <= 0)
                NextStone();
        }

        private void OnStoneClickNetwork(int damage) // 4
        {
            if (damage == 0)
            {
                return;
            }
            TakeDamage(damage);
            _healthBarUIService.UpdateHealthBarState(_currentHealth, GetHealth(_currentLocation, _currentStone));

            _stoneAnimatorEventsInvoke.OnStoneClickPlayInvoke();
        }

        private void TakeDamage(int damage) // 1
        {
            Debug.Log("Current health " + _currentHealth);

            OnHealthChanged?.Invoke();
        }

        private void AddCLickOnServer() //6
        {
            Debug.Log("Add click on Server");

            PlayerBehavior playerBehavior = PowerButton.PlayerBehavior;
            int damage = playerBehavior.playerDamageOnServer;
            playerBehavior.playerDamageOnServer = 1;
            CmdAddClickToServer(damage);
        }

        [Command(requiresAuthority = false)]
        private void CmdAddClickToServer(int damage, NetworkConnectionToClient target = null) // 2
        {
            _countClickAllCallback += damage;

            _currentHealth = Mathf.Max(0, _currentHealth - damage);

            Debug.Log("Take dmg on server " + damage);
            TargetCallbackOnClick(target, damage);

            if (_currentHealth <= 0)
                NextStone();
        }

        [TargetRpc]
        private void TargetCallbackOnClick(NetworkConnectionToClient target, int damage) // 7
        {
            OnStoneClick(damage);
        }

        [ClientRpc]
        private void OnStoneCallbackNetwork(int damage)
        {
            int clearCount = damage - _countDamageAfterCallback; // 5

            //Debug.Log($"Get server callback. All count {count}. Clear count {clearCount}");
            OnStoneClickNetwork(clearCount);
            _countDamageAfterCallback = 0;
        }

        private IEnumerator IStoneServerCallbackProcess()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(_serverCallBackTime);
                OnStoneCallbackNetwork(_countClickAllCallback);
                _countClickAllCallback = 0; // 8
                SaveData();
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
            _stoneData = (StoneData)_stoneSaveSystem.Load();
            if (_stoneData == null)
            {
                _stoneData = new StoneData();
                _stoneSaveSystem.Save(_stoneData);
            }

            _currentLocation = _stoneData.CurrentLocation;
            _currentStone = _stoneData.CurrentStone;
        }

        private void LoadHealth()
        {
            _healthSaveSystem ??= new BinarySaveSystem(HEALTH_DATA_PATCH);
            _healthData = (HealthData)_healthSaveSystem.Load();
            if (_healthData == null)
            {
                _healthData = new HealthData();
                for (int i = 0; i < _getLocationCountService.GetLocationsCount(); i++)
                {
                    _healthData.Healths.Add(new());
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

            _currentHealth = (_stoneData.CurrentHealth != 0) ? _stoneData.CurrentHealth : _hpPerLvl * (_currentStone + 1);
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
            for (int i = _currentStone + 1; i < _hpList[_currentLocation].Count; i++)
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
            CmdSaveHealth(location, lvl, value, _networkIdentity.connectionToClient);
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
        private void CmdSaveHealth(int location, int lvl, int value, NetworkConnectionToClient target = null)
        {
            _hpList[location][lvl] = value;
            Debug.Log(_hpList[location][lvl]);
            _healthData.Healths[location].Array[lvl] = value;
            SaveHealth();
            RpcSetHealth(location, lvl, value);
        }

        [ClientRpc]
        private void RpcSetHealth(int location, int lvl, int value)
        {
            _hpList[location][lvl] = value;
        }

        [Command(requiresAuthority = false)]
        private void CmdSaveCurrentHealth(int value, NetworkConnectionToClient target = null)
        {
            _currentHealth = value;
            _countClickAllCallback = 0;
            RpcSetCurrentHealth(value);
        }

        [ClientRpc]
        private void RpcSetCurrentHealth(int value)
        {
            _currentHealth = value;
            _countDamageAfterCallback = 0;
        }

        public void ChangeLocation(int location, int stone)
        {
            CmdChangeLocation(location, stone, _networkIdentity.connectionToClient);
        }

        [Command(requiresAuthority = false)]
        private void CmdChangeLocation(int location, int stone, NetworkConnectionToClient target = null) //TODO: Не работает обновление камня на сервере
        {
            _currentLocation = location;
            _currentStone = stone;
            _currentHealth = (_stoneData.CurrentHealth != 0) ? _stoneData.CurrentHealth : _hpPerLvl * (_currentStone + 1);
            SaveData();
            AllLoadGameTarget(_currentLocation, _currentStone);
            AllStartGameTarget();
        }
    }
}