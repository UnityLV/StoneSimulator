using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.RemoteConfig;
using Firebase.Unity;
using UnityEngine;
using UnityEngine.Events;

namespace FirebaseCustom
{
    public class RemoteConfigLoader 
    {
        public async Task<Config> Load()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync();
            FirebaseRemoteConfig remoteConfig =
                FirebaseRemoteConfig.GetInstance(FirebaseApp.Create());

            await remoteConfig.FetchAsync(TimeSpan.Zero);
            await remoteConfig.ActivateAsync();
            Config config = new Config();
            config.SetFromFirebaseRemoteConfig(remoteConfig);
            return config;
        }
    }
}