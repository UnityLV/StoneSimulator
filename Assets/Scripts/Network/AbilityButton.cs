using System;
using System.Threading.Tasks;
using PlayerData.Interfaces;
using Stone.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace GameScene
{
    public class AbilityButton : MonoBehaviour, IAbilityClickEvents
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _counterText;
        [SerializeField] private UnityEvent _emptyAbilityClick;
        private IStoneClickEvents _stoneClickEvents;
        private IStoneClickEventsInvoke _clickEventsInvoke;
        private IClickDataService _clickDataService;
        private int _counter;
        private bool _isNeedToIgnoreOneInputClick;

        public static PlayerBehavior PlayerBehavior { get; set; }
        public event Action<int> OnAbilityClick;


        [Inject]
        private void Construct(IStoneClickEvents stoneClickEvents, IStoneClickEventsInvoke clickEventsInvoke, IClickDataService clickDataService)
        {
            _stoneClickEvents = stoneClickEvents;
            _clickEventsInvoke = clickEventsInvoke;
            _clickDataService = clickDataService;
        }

        private void OnEnable()
        {
            _stoneClickEvents.OnStoneClick += OnOnStoneClick;
            _button.onClick.AddListener(OnButtonClick);
        }


        private void OnDisable()
        {
            _stoneClickEvents.OnStoneClick -= OnOnStoneClick;
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnOnStoneClick()
        {
            if (_isNeedToIgnoreOneInputClick)
            {
                _isNeedToIgnoreOneInputClick = false;
                return;
            }
            _counter++;
            _counterText.text = _counter.ToString();
        }

        private void OnButtonClick()
        {
            if (_counter <= 0)
            {
                _emptyAbilityClick?.Invoke();
                return;
            }
            ProcessAbilityUse();
        }

        private async void ProcessAbilityUse()
        {
            _isNeedToIgnoreOneInputClick = true;
            int clicksInAbility = GetAndResetCounter();

            PlayerBehavior.SetDamage(clicksInAbility);

            await Task.Delay(100); //HACK: wait for damage Update;

            _clickEventsInvoke.OnStoneClickInvoke();
            _clickDataService.AddClicks(Mathf.Clamp(clicksInAbility - 1, 0, int.MaxValue)); //HACK: minus 1 bcs click on stone from ability added 1 additivly
            OnAbilityClick?.Invoke(Mathf.Clamp(clicksInAbility - 1, 0, int.MaxValue));
        }

        public int GetAndResetCounter()
        {
            int temp = _counter;
            _counter = 0;
            _counterText.text = _counter.ToString();
            return temp;
        }
    }
}