﻿using Firebase.RemoteConfig;
using UnityEngine;

namespace FirebaseCustom
{
    /// <summary>
    /// Remote Config from FireBase
    /// </summary>
    public struct Config
    {
        public string UnityServerIp { get; private set; }
        public string UnityServerPort { get; private set; }
        public string DBServerIp { get; private set; }
        public string DBServerPort { get; private set; }
        public int ClicksToRedeemed { get; private set; }
        public int PercentToAddToReferrer { get; private set; }
        public string RanksJson { get; private set; }
        public int EarnedFromEachReferral { get; private set; }
        public int ClicksFromAD { get; private set; }

        private FirebaseRemoteConfig _remoteConfig;

        public void SetFromFirebaseRemoteConfig(FirebaseRemoteConfig remoteConfig)
        {
            _remoteConfig = remoteConfig;

            UnityServerIp = GetString(ConfigKeys.UnityServerIp);
            UnityServerPort = GetString(ConfigKeys.UnityServerPort);
            DBServerIp = GetString(ConfigKeys.DBServerIp);
            DBServerPort = GetString(ConfigKeys.DBServerPort);
            ClicksToRedeemed = GetInt(ConfigKeys.ClicksToRedeemed);
            PercentToAddToReferrer = GetInt(ConfigKeys.PercentToAddToReferrer);
            RanksJson = GetString(ConfigKeys.Ranks);
            EarnedFromEachReferral = GetInt(ConfigKeys.EarnedFromEachReferral);
            ClicksFromAD = GetInt(ConfigKeys.ClicksFromAD);
            DebugValues();
        }

        private void DebugValues()
        {
            Debug.Log("Remote Values ==================================================");
            Debug.Log($"UnityServerIp: {UnityServerIp}");
            Debug.Log($"UnityServerPort: {UnityServerPort}");
            Debug.Log($"DBServerIp: {DBServerIp}");
            Debug.Log($"DBServerPort: {DBServerPort}");
            Debug.Log($"ClicksToRedeemed: {ClicksToRedeemed}");
            Debug.Log($"PercentToAddToReferrer: {PercentToAddToReferrer}");
            Debug.Log($"RanksJson: {RanksJson}");
            Debug.Log($"EarnedFromEachReferral: {EarnedFromEachReferral}");
            Debug.Log($"ClicksFromAD: {ClicksFromAD}");
            Debug.Log("==================================================");
        }

        private float GetFloat(string key)
        {
            return (float)_remoteConfig.GetValue(key).DoubleValue;
        }

        private string GetString(string key)
        {
            return _remoteConfig.GetValue(key).StringValue;
        }

        private int GetInt(string key)
        {
            return (int)_remoteConfig.GetValue(key).DoubleValue;
        }
    }

    public static class ConfigKeys
    {
        public const string HpPerLevel = "LocationHealthPerLevel";
        public const string DBServerIp = "DBServerIp";
        public const string DBServerPort = "DBServerPort";
        public const string UnityServerIp = "UnityServerIp";
        public const string UnityServerPort = "UnityServerPort";
        public const string ClicksToRedeemed = "ClicksToRedeemed";
        public const string PercentToAddToReferrer = "PercentToAddToReferrer";
        public const string Ranks = "Ranks";
        public const string EarnedFromEachReferral = "EarnedFromEachReferral";
        public const string ClicksFromAD = "ClicksFromAD";
    }
}