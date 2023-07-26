using MongoDBCustom;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;
namespace InGameUI
{
    public class TestMarketLogic : MonoBehaviour, IMarketLogic
    {
        private IClickDataService _clickDataService;
        private IDBAllClickSaver _dbAllClickSaver;

        [Inject]
        private void Construct(IClickDataService clickDataService, IDBAllClickSaver dbAllClickSaver)
        {
            _clickDataService = clickDataService;
            _dbAllClickSaver = dbAllClickSaver;

     
        }
  
        public void BuyClicks()
        {
            int add = 2500;
            _clickDataService.AddClicks(add);
            _dbAllClickSaver.Save(add);
            Debug.Log("Add CLicks " + add);
        }

        public void RemoveAD()
        {
            Debug.Log("remove AD");
        }

        public void PinMessage()
        {
            Debug.Log("Pin Message");
        }
    }
}