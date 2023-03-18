using System;
using System.Collections;
using System.Threading.Tasks;
using GameScene;
using GameScene.Interfaces;
using Network.Enum;
using Network.Interfaces;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        #region Dependency

        private IGameSceneService _gameSceneService;
        private INetworkManagerService _networkManagerService;

        [Inject]
        private void Construct(IGameSceneService gameSceneService, INetworkManagerService networkManagerService)
        {
            _gameSceneService = gameSceneService;
            _networkManagerService = networkManagerService;
        }

        #endregion

        private void Start()
        {
            StartCoroutine(ILoadSceneAwait());
        }

        private IEnumerator ILoadSceneAwait()
        {
            yield return new WaitForSecondsRealtime(1);
            _gameSceneService.BeginLoadGameScene(GameSceneType.Game);
            Debug.Log(_networkManagerService);
            if (_networkManagerService.GetConnectionType() == ConnectionType.Server)
                _gameSceneService.BeginTransaction();
        }

        public void OnLocationButtonClick()
        {
            _gameSceneService.BeginTransaction();
        }
    }
}