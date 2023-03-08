using System;
using GameScene;
using GameScene.Interfaces;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        #region Dependency

        private IGameSceneService _gameSceneService;
        
        [Inject]
        private void Construct(IGameSceneService gameSceneService)
        {
            _gameSceneService = gameSceneService;
        }

        #endregion

        private void Start()
        {
            _gameSceneService.BeginLoadGameScene(GameSceneType.Game);
        }

        public void OnLocationButtonClick()
        {
            _gameSceneService.BeginTransaction();
        }
    }
}