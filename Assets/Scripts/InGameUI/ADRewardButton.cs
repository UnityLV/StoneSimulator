using AYellowpaper;
using UnityEngine;


namespace InGameUI
{


    public class ADRewardButton : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IADReward> _adRewardSystem;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ShowReward();
            }
        }

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