using System.Collections;
using System.Threading.Tasks;
using GameScene;
using GameScene.Interfaces;
using UnityEngine;
using Zenject;

namespace InGameUI
{
    public class InGameUI: MonoBehaviour
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
            StartCoroutine(ILoadSceneAwait());
        }

        private IEnumerator ILoadSceneAwait()
        {
            yield return new WaitForSecondsRealtime(1);
            _gameSceneService.BeginLoadGameScene(GameSceneType.MainMenu);
        }

        public void OnHomeClick()
        {
            _gameSceneService.BeginTransaction();
        }
    }
}