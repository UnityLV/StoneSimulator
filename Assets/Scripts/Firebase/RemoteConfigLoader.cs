using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.RemoteConfig;

namespace FirebaseCustom.Loaders
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