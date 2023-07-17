using UnityEngine;

namespace FirebaseCustom
{
    [CreateAssetMenu]
    public class MongoDBConnectionConfig : ConnectionConfig
    {
        public string GetConnection()
        {
            return $"mongodb://admin:10285967@{Ip}:{Port}";
            
        }
    }
}