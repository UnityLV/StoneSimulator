using UnityEngine;

namespace FirebaseCustom
{
    [CreateAssetMenu]
    public class MongoDBConnectionConfig : ConnectionConfig
    {
        public string GetConnectionString()
        {
            
#if UNITY_ANDROID
            return $"mongodb://{Ip}:{Port}";
#else
            return $"mongodb://localhost:27017";
#endif
        }
    }
}