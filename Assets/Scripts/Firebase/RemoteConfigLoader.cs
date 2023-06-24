using System;
using System.Collections.Generic;
using Firebase;
using Firebase.RemoteConfig;
using Firebase.Unity;
using UnityEngine;
using UnityEngine.Events;

namespace FirebaseCustom
{
    public class RemoteConfigLoader : MonoBehaviour
    {
        public event UnityAction<Config> Loaded;

        async void Awake()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync();
            FirebaseRemoteConfig remoteConfig =
                FirebaseRemoteConfig.GetInstance(FirebaseApp.Create());

            await remoteConfig.FetchAsync(TimeSpan.Zero);
            await remoteConfig.ActivateAsync();
            Config config = new Config();
            config.SetFromFirebaseRemoteConfig(remoteConfig);


            Debug.Log("Remote Config Loaded" + JsonUtility.ToJson(config) );
            Loaded?.Invoke(config);
        }
    }
}