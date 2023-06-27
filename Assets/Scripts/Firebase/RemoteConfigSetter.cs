using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FirebaseCustom
{
    public class RemoteConfigSetter
    {
        public RemoteConfigSetter(ConnectionConfig unityServerConnectionConfig,
            ConnectionConfig dbConnectionConfig)
        {
            this._unityServerConnectionConfig = unityServerConnectionConfig;
            this._dbConnectionConfig = dbConnectionConfig;
        }

        private readonly ConnectionConfig _unityServerConnectionConfig;
        private readonly ConnectionConfig _dbConnectionConfig;

        public void SetConfigs(Config config)
        {
            _unityServerConnectionConfig.Ip = config.UnityServerIp;
            _unityServerConnectionConfig.Port = config.UnityServerPort;
            _dbConnectionConfig.Ip = config.DBServerIp;
            _dbConnectionConfig.Port = config.DBServerPort;
        }
    }
}