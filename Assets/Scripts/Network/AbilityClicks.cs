using System;
using System.Threading.Tasks;
using I2.Loc;
using PlayerData.Interfaces;
using Stone.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace GameScene
{
    public class AbilityClicks : MonoBehaviour, IAbilityClickEvents
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text[] _counterTexts;
        [SerializeField] private TMP_Text _specialText;
        [SerializeField] private UnityEvent _emptyAbilityClick;
        private IStoneClickEvents _stoneClickEvents;
        private IStoneClickEventsInvoke _clickEventsInvoke;
        private IClickDataService _clickDataService;

        private const string CounterKey = nameof(CounterKey);

        public int Counter
        {
            get => PlayerPrefs.GetInt(CounterKey);

            set => PlayerPrefs.SetInt(CounterKey, value);
        }

        public static PlayerBehavior PlayerBehavior { get; set; }
        public event Action<int> OnAbilityClick;

        [Inject]
        private void Construct(IStoneClickEvents stoneClickEvents, IStoneClickEventsInvoke clickEventsInvoke, IClickDataService clickDataService)
        {
            _stoneClickEvents = stoneClickEvents;
            _clickEventsInvoke = clickEventsInvoke;
            _clickDataService = clickDataService;
        }

        private void Awake()
        {
            LocalizationManager.OnLocalizeEvent += OnUpdateLanguage;
            _stoneClickEvents.OnStoneClick += OnOnStoneClick;
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            LocalizationManager.OnLocalizeEvent -= OnUpdateLanguage;
            _stoneClickEvents.OnStoneClick -= OnOnStoneClick;
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnUpdateLanguage()
        {
            UpdateSpecialText();
        }
        
        public void AddClicks(int clicks)
        {
            Counter += clicks;

            UpdateAllTexts();
        }

        private void UpdateSpecialText()
        {
            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                _specialText.text = $"Вы собрали {Counter.ToString()} кликов";
                return;
            }
            _specialText.text = $"You have collected {Counter.ToString()} clicks";
            
          
        }

        private void UpdateAllTexts()
        {
            foreach (var text in _counterTexts)
            {
                text.text = Counter.ToString();
            }

            UpdateSpecialText();
        }

        private void OnOnStoneClick()
        {
            //AddClicks(1);
        }

        private void OnButtonClick()
        {
            if (Counter <= 0)
            {
                _emptyAbilityClick?.Invoke();
                return;
            }
            ProcessAbilityUse();
        }

        private async void ProcessAbilityUse()
        {
            int clicksInAbility = GetAndResetCounter();

            PlayerBehavior.SetDamage(clicksInAbility);

            await Task.Delay(100); //HACK: wait for damage Update;

            _clickEventsInvoke.OnStoneClickInvoke();
            _clickDataService.AddClicks(Mathf.Clamp(clicksInAbility - 1, 0, int.MaxValue)); //HACK: minus 1 bcs click on stone from ability added 1 additivly
            OnAbilityClick?.Invoke(Mathf.Clamp(clicksInAbility - 1, 0, int.MaxValue));
        }

        public int GetAndResetCounter()
        {
            int temp = Counter;
            Counter = 0;

            UpdateAllTexts();
          
            return temp;
        }
    }
}