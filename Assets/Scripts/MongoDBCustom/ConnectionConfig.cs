using UnityEngine;

namespace FirebaseCustom
{
    [CreateAssetMenu]
    public class ConnectionConfig : ScriptableObject
    {
        [field: SerializeField] public string Ip { get;  set; }
        [field: SerializeField] public string Port { get;  set; }
    }
}