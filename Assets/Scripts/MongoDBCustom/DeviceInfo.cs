using UnityEngine;

namespace MongoDBCustom
{
    public static class DeviceInfo
    {
        public static string GetDeviceId()
        {
            string deviceId = "";
#if UNITY_ANDROID
            deviceId = SystemInfo.deviceUniqueIdentifier;
#elif UNITY_IOS
        deviceId = SystemInfo.deviceIdentifier;
#endif
            return deviceId;
        }
    }
}