using AYellowpaper;
using UnityEngine;
namespace InGameUI
{
    public class Market : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IMarketLogic> _marketLogic;
        
        
        public void BuyClicks()
        {
            _marketLogic.Value.BuyClicks();
        }
        
        public void RemoveAD()
        {
            _marketLogic.Value.RemoveAD();
        }
        
        public void PinMessage()
        {
            _marketLogic.Value.PinMessage();
        }
        
    }

}