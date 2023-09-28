using UnityEngine;

namespace FirebaseCustom
{
    [CreateAssetMenu]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public int ClicksToRedeemed { get; set; } = 0;
        [field: SerializeField] public int PercentToAddToReferrer { get;  set; }= 0;
        [field: SerializeField] public int EarnedFromEachReferral { get;  set; }= 0;
        [field: SerializeField] public int ClicksFromAD { get;  set; }= 0;
    }
}