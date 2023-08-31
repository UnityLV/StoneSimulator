using UnityEngine;

namespace FirebaseCustom
{
    [CreateAssetMenu]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public int ClicksToRedeemed { get;  set; }
        [field: SerializeField] public int PercentToAddToReferrer { get;  set; }
        [field: SerializeField] public int EarnedFromEachReferral { get;  set; }
    }
}