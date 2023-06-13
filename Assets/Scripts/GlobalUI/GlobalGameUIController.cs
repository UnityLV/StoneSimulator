using System;
using GameState.Interfaces;
using InGameUI.Interfaces;
using MainMenuUI.Inrefaces;
using MongoDBCustom;
using UnityEngine;
using Zenject;

namespace GlobalUI
{
    public class GlobalGameUIController:MonoBehaviour
    {
        #region Dependency

        private IInGameService _inGameService;
        private IMainMenuService _mainMenuService;
        private IGameStateService _gameStateService;
        private IDBRatingSaver _dbRatingSaver;
        
        [Inject]
        private void Construct(IInGameService inGameService, IMainMenuService mainMenuService, IGameStateService gameStateService,IDBRatingSaver dbRatingSaver)
        {
            _inGameService = inGameService;
            _mainMenuService = mainMenuService;
            _gameStateService = gameStateService;
            _dbRatingSaver = dbRatingSaver;
        }

        #endregion

        private void Start()
        {
            _inGameService.SetState(false,false);
            _mainMenuService.SetState(true);
            
            _inGameService.SetOnHomeClickAction(() =>
            {
                _inGameService.SetState(false,false);
                _mainMenuService.SetState(true);
                _dbRatingSaver.SaveRating();
            });
            
            _mainMenuService.SetInProgressLocationClickAction(() =>
            {
                _inGameService.SetState(true,true);
                _mainMenuService.SetState(false);
                _gameStateService.TryStartGame();
            });
            
            _mainMenuService.SetOnCompleteLocationClickAction((x) =>
            {
                _inGameService.SetState(true,false);
                _mainMenuService.SetState(false);
                _gameStateService.TryWatchLocation(x);
            });
        }
    }
}