using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FirebaseCustom
{
    public class RemoteConfigSetter : MonoBehaviour
    {
        [SerializeField] private ConnectionConfig unityServerConnectionConfig;
        [SerializeField] private MongoDBConnectionConfig dbConnectionConfig;
        
        [SerializeField] private RemoteConfigLoader _loader;

        [SerializeField] private UnityEvent _set; 

        private void OnEnable()
        {
            _loader.Loaded += LoaderOnLoaded;
        }

        private void OnDisable()
        {
            _loader.Loaded -= LoaderOnLoaded;
        }

        private void LoaderOnLoaded(Config config)
        {
            unityServerConnectionConfig.Ip = config.UnityServerIp;
            unityServerConnectionConfig.Port = config.UnityServerPort;
            dbConnectionConfig.Ip = config.DBServerIp;
            dbConnectionConfig.Port = config.DBServerPort;
          
            _set?.Invoke();
         
        }
    }
}