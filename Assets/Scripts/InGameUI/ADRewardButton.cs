using AYellowpaper;
using UnityEngine;


namespace InGameUI
{


    public class ADRewardButton : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IADReward> _adRewardSystem;

        public void ShowReward()
        {
            _adRewardSystem.Value.ShowReward();
        }
    }

    public interface IADReward
    {
        void ShowReward();
        void OnComplete();
    }
}