using UnityEngine;

namespace FirebaseCustom
{
    [CreateAssetMenu]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public int ClicksToRedeemed { get;  set; }
    }
}