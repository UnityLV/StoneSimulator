using System;
using MongoDBCustom;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;
namespace InGameUI
{
    public class TestMarketLogic : MonoBehaviour, IMarketLogic
    {


        public void BuyClicks(Action callback)
        {
            callback();
        }

        public void BuyRemoveAD(Action callback)
        {
            callback();
        }

        public void BuyPinMessage(Action callback)
        {
            callback();
        }
    }
}