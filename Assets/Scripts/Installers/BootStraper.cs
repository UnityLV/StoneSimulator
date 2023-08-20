﻿using System;
using System.Threading.Tasks;
using FirebaseCustom.Loaders;
using FirebaseCustom;
using MongoDBCustom;
using Network;
using PlayerData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Installers
{
    public class BootStraper : MonoBehaviour
    {
        [SerializeField] private CustomNetworkManager _customNetworkManager;

        [SerializeField] private MongoDBConnectionConfig _db;
        [SerializeField] private ConnectionConfig _mirror;
        [SerializeField] private PlayerConfig playerConfig;




        [SerializeField] private PlayerDataSetter _playerDataSetter;


        private IdbCommands _idbCommands;

        private async void Start()
        {
#if !UNITY_SERVER
            await LoadRemoteConfig();
            Debug.Log("RemoteConfig Loaded");
#endif

            var connection = await ConstructConnection();
            _idbCommands = new IdbCommands(connection);
            ValuesFromBootScene.IdbCommands = _idbCommands;
            ValuesFromBootScene.MongoConnection = connection;

            await _playerDataSetter.SetData();

            _customNetworkManager.TryConnect();
        }

        private async Task<IMongoConnection> ConstructConnection()
        {
            IMongoConnection connection = new MongoDBConnect(_db).GetConnectionData();
            MongoDBConnector connector = new MongoDBConnector(connection);
            await connector.TryConnect();
            return connection;
        }

        private async Task LoadRemoteConfig()
        {
            RemoteConfigSetter remoteConfigSetter = new RemoteConfigSetter(_mirror, _db, playerConfig);
            Config config = await new RemoteConfigLoader().Load();
            remoteConfigSetter.SetConfig(config);
        }
    }
}