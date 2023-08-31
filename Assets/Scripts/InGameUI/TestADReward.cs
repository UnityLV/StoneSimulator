using System.Threading.Tasks;
using FirebaseCustom;
using GameScene;
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
        private AbilityButton _abilityButton;
        PlayerConfig PlayerConfig = RemoteConfigSetter.PlayerConfig;

        [Inject]
        private void Construct(IClickDataService clickDataService, IDBAllClickSaver dbAllClickSaver, AbilityButton abilityButton)
        {
            _clickDataService = clickDataService;
            _dbAllClickSaver = dbAllClickSaver;
            _abilityButton = abilityButton;
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
            int add = PlayerConfig.ClicksFromAD;
            // _clickDataService.AddClicks(add);
            // _dbAllClickSaver.Save(add);
            _abilityButton.AddClicks(add);
            Debug.Log("Reward CLicks " + add);

            
        }
    }
}