using System;
using GameState.Interfaces;
using MainMenuUI.Inrefaces;
using MainMenuUI.LocationMainMenu;
using PlayerData.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace MainMenuUI
{
    public class MainMenuController : MonoBehaviour,IMainMenuService
    {
        [SerializeField]
        private TMP_InputField _nicknameText;

        [SerializeField]
        private TextMeshProUGUI _currentHealth;

        [SerializeField]
        private GameObject _mainMenuUI;

        [SerializeField]
        private LocationMainMenuController _locationMainMenuController;

        
        #region Dependency

        private INicknameDataService _nicknameDataService;
        private IHealthService _healthService;
        private IGameStateCallbacks _gameStateCallbacks;
        
        [Inject]
        private void Construct(
            INicknameDataService nicknameDataService,
            IHealthService healthService,
            IGameStateCallbacks gameStateCallbacks)
        {
            _nicknameDataService = nicknameDataService;
            _healthService = healthService;
            _gameStateCallbacks = gameStateCallbacks;
        }

        #endregion

        public void SubscribeCallbacks()
        {
            _gameStateCallbacks.OnHealthChanged += UpdateHealth;
        }

        public void UnsubscribeCallbacks()
        {
            _gameStateCallbacks.OnHealthChanged -= UpdateHealth;
        }
        
        private void Start()
        {
            UpdateNicknameText();
            SubscribeCallbacks();
        }

        public void UpdateNicknameText()
        {
            _nicknameText.text = _nicknameDataService.GetNickname();
        }

        public void UpdateHealth()
        {
            _currentHealth.text =
                $" {_healthService.GetCurrentLocationHealth()}/{_healthService.GetMaxLocationHealth()} кликов";
        }

        public void SetState(bool state)
        {
            _mainMenuUI.SetActive(state);
            if (state)
            {
                SubscribeCallbacks();
                _locationMainMenuController.SubscribeCallbacks();
                try
                {
                    _locationMainMenuController.UpdateLocationUI();
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                UnsubscribeCallbacks();
                _locationMainMenuController.UnsubscribeCallbacks();
            }
        }

        public void SetOnCompleteLocationClickAction(Action<int> action)
        {
            _locationMainMenuController.SetOnCompleteLocationClickAction(action); 
        }

        public void SetInProgressLocationClickAction(Action action)
        {
            _locationMainMenuController.SetInProgressLocationClickAction(action);
        }
    }
}