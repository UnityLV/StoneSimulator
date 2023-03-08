using GameScene.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScene
{
    public class GameSceneController : IGameSceneService
    {
        private static AsyncOperation _asyncOperation = null;

        private string _currentAsyncLoad = "";

        public void BeginLoadGameScene(GameSceneType state)
        {
            _currentAsyncLoad = state switch
            {
                GameSceneType.Game => "GameplayScene",
                GameSceneType.MainMenu => "MainMenuScene",
                _ => _currentAsyncLoad
            };

            _asyncOperation = SceneManager.LoadSceneAsync(_currentAsyncLoad);
            _asyncOperation.allowSceneActivation = false;
        }

        public void BeginTransaction()
        {
            _currentAsyncLoad = "";
            _asyncOperation.allowSceneActivation = true;
            _asyncOperation = null;
        }
    }
}