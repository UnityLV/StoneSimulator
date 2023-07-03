using UnityEngine;

namespace FirebaseCustom
{
    [CreateAssetMenu]
    public class MongoDBConnectionConfig : ConnectionConfig
    {
        public string GetConnectionString()
        {
            return $"mongodb://{Ip}:{Port}";
        }
    }
}