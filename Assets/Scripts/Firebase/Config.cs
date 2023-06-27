using Firebase.RemoteConfig;

namespace FirebaseCustom
{
    public struct Config
    {
        public string UnityServerIp;
        public string UnityServerPort;
        public string DBServerIp;
        public string DBServerPort;

        public void SetFromFirebaseRemoteConfig(FirebaseRemoteConfig remoteConfig)
        {
            UnityServerIp = remoteConfig.GetValue(ConfigKeys.UnityServerIp).StringValue;
            UnityServerPort = remoteConfig.GetValue(ConfigKeys.UnityServerPort).StringValue;
            DBServerIp = remoteConfig.GetValue(ConfigKeys.DBServerIp).StringValue;
            DBServerPort = remoteConfig.GetValue(ConfigKeys.DBServerPort).StringValue;
        }
    }

    public static class ConfigKeys
    {
        public const string DBServerIp = "DBServerIp";
        public const string DBServerPort = "DBServerPort";
        public const string UnityServerIp = "UnityServerIp";
        public const string UnityServerPort = "UnityServerPort";
    }
}