using System;
namespace InGameUI
{
    public interface IMarketLogic
    {
        void BuyClicks(Action callback);
        
        void BuyRemoveAD(Action callback);
        
        void BuyPinMessage(Action callback);
    }
}