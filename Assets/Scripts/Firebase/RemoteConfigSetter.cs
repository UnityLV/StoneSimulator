using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FirebaseCustom
{
    public class RemoteConfigSetter
    {
        private readonly ConnectionConfig _unityServerConnectionConfig;
        private readonly ConnectionConfig _dbConnectionConfig;
        private readonly PlayerConfig _playerConfig;


        public RemoteConfigSetter(ConnectionConfig unityServerConnectionConfig, ConnectionConfig dbConnectionConfig, PlayerConfig playerConfig)
        {
            _unityServerConnectionConfig = unityServerConnectionConfig;
            _dbConnectionConfig = dbConnectionConfig;
            _playerConfig = playerConfig;
        }

        public void SetConfigs(Config config)
        {
            _unityServerConnectionConfig.Ip = config.UnityServerIp;
            _unityServerConnectionConfig.Port = config.UnityServerPort;
            _dbConnectionConfig.Ip = config.DBServerIp;
            _dbConnectionConfig.Port = config.DBServerPort;
            _playerConfig.ClicksToRedeemed = config.ClicksToRedeemed;
        }
    }
}