using System.Threading.Tasks;
using MongoDBCustom;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;
namespace InGameUI
{

    public class TestADReward : MonoBehaviour, IADReward
    {
        [SerializeField] private GameObject _adWindow;
        
        private IClickDataService _clickDataService;
        private IDBAllClickSaver _dbAllClickSaver;

        [Inject]
        private void Construct(IClickDataService clickDataService, IDBAllClickSaver dbAllClickSaver)
        {
            _clickDataService = clickDataService;
            _dbAllClickSaver = dbAllClickSaver;
        }
        public async void ShowReward()
        {
            _adWindow.gameObject.SetActive(true);

            await Task.Delay(1000);

            OnComplete();
        }
        public void OnComplete()
        {
            _adWindow.gameObject.SetActive(false);
            AddClicks();
        }
        
        private void AddClicks()
        {
            int add = 100;
            _clickDataService.AddClicks(add);
            _dbAllClickSaver.Save(add);
            Debug.Log("Reward CLicks " + add);
        }
    }
}