using GameState.Interfaces;
using InGameUI.Interfaces;
using MainMenuUI;
using MainMenuUI.Inrefaces;
using MongoDBCustom;
using UnityEngine;
using Zenject;

namespace GlobalUI
{
    public class GlobalGameUIController : MonoBehaviour
    {
        [SerializeField] private LocationWindowManager _locationWindow;

        #region Dependency

        private IInGameService _inGameService;
        private IMainMenuService _mainMenuService;
        private IGameStateService _gameStateService;
        private IDBAllClickSaver _allClickSaver;
        private IDBReferrerClicksSaver _referrerClicks;

        [Inject]
        private void Construct(IInGameService inGameService, IMainMenuService mainMenuService,
            IGameStateService gameStateService, IDBAllClickSaver idbAllClickSaver,
            IDBReferrerClicksSaver referrerClicks)
        {
            _inGameService = inGameService;
            _mainMenuService = mainMenuService;
            _gameStateService = gameStateService;
            _allClickSaver = idbAllClickSaver;
            _referrerClicks = referrerClicks;
        }

        #endregion

        private void OnDisable()
        {
            _allClickSaver?.Save();
            _referrerClicks?.Save();
        }

        private void Start()
        {
            _inGameService.SetState(false, false);
            _mainMenuService.SetState(true);

            _inGameService.SetOnHomeClickAction(() =>
            {
                _inGameService.SetState(false, false);
                _mainMenuService.SetState(true);
                _allClickSaver.Save();
                _referrerClicks.Save();
            });

            _mainMenuService.SetInProgressLocationClickAction(() =>
            {
                _inGameService.SetState(true, true);
                _mainMenuService.SetState(false);
                _gameStateService.TryStartGame();
            });

            _mainMenuService.SetOnCompleteLocationClickAction((locationId) =>
                _locationWindow.ShowWindow(locationId, OpenCompleteLocation)
            );
        }

        private void OpenCompleteLocation(int x)
        {
            _inGameService.SetState(true, false);
            _mainMenuService.SetState(false);
            _gameStateService.TryWatchLocation(x);
            _allClickSaver.Save();
            _referrerClicks.Save();
        }
    }
}