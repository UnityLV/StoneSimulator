using GameScene;
using GameScene.Interfaces;
using UnityEngine;
using Zenject;

public class LoadingServer : MonoBehaviour
{
    #region Dependency

    private IGameSceneService _gameSceneService;
    
    [Inject]
    private void Construct(IGameSceneService gameSceneService)
    {
        _gameSceneService = gameSceneService;
    }

    #endregion
 
    
    void Start()
    {
        

        _gameSceneService.BeginLoadGameScene(GameSceneType.MainMenu);
    }
}
