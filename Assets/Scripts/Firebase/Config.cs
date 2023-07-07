using Firebase.RemoteConfig;

namespace FirebaseCustom
{
    public struct Config
    {
        public string UnityServerIp { get; private set; }
        public string UnityServerPort { get; private set; }
        public string DBServerIp { get; private set; }
        public string DBServerPort { get; private set; }
        public int ClicksToRedeemed { get; private set; }


        private FirebaseRemoteConfig _remoteConfig;

        public void SetFromFirebaseRemoteConfig(FirebaseRemoteConfig remoteConfig)
        {
            _remoteConfig = remoteConfig;

            UnityServerIp = GetString(ConfigKeys.UnityServerIp);
            UnityServerPort = GetString(ConfigKeys.UnityServerPort);
            DBServerIp = GetString(ConfigKeys.DBServerIp);
            DBServerPort = GetString(ConfigKeys.DBServerPort);
            ClicksToRedeemed = GetInt(ConfigKeys.ClicksToRedeemed);
        }

        private string GetString(string key)
        {
            return _remoteConfig.GetValue(key).StringValue;
        }

        private int GetInt(string key)
        {
            return (int) _remoteConfig.GetValue(key).DoubleValue;
        }
    }

    public static class ConfigKeys
    {
        public const string DBServerIp = "DBServerIp";
        public const string DBServerPort = "DBServerPort";
        public const string UnityServerIp = "UnityServerIp";
        public const string UnityServerPort = "UnityServerPort";
        public const string ClicksToRedeemed = "ClicksToRedeemed";
    }
}