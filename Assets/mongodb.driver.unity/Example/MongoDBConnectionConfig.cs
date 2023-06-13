using UnityEngine;

namespace MongoDBCustom
{
    [CreateAssetMenu]
    public class MongoDBConnectionConfig : ScriptableObject
    {
        [SerializeField] private string _ip;
        [SerializeField] private string _port;
        
        public string GetConnectionString()
        {
            return $"mongodb://{_ip}:{_port}";
        }
    }
}