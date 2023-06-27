using CameraRotation;
using GameState;
using Health;
using Health.Interfaces;
using InGameUI;
using UnityEngine;
using Zenject;
using InputService;
using InputService.Interfaces;
using LocationGameObjects;
using LocationGameObjects.Interfaces;
using MainMenuUI;
using MainMenuUI.LocationMainMenu;
using MongoDBCustom;
using Network.Interfaces;
using Stone;
using UnityEngine.Serialization;

namespace Installers
{
    public class GameSceneLocationInstaller : MonoInstaller
    {
        [SerializeField] private InputController _inputController;

        [SerializeField] private LocationsObjectDataHolder _locationsObjectDataHolder;

        [SerializeField] private HealthBarUIController _healthBarUIController;

        [SerializeField] private GameStateMachine _gameStateMachine;

        [SerializeField] private InGameUIController _inGameUIController;

        [SerializeField] private MainMenuController _mainMenuController;

        [SerializeField] private CameraRotationObject _cameraRotation;
        [SerializeField] private InGameRatingListUI _ratingListUI;

        [SerializeField] private PlayerRatingInDBSaver _playerRatingInDBSaver;


        public override void InstallBindings()
        {
            BindController();
            BindLocationDataHolder();
            BindLocationFactory();
            BindStoneEventClick();
            BindUIHealthBar();
            BindNetworkManager();
            BindLocationMainMenuFactory();
            BindGameStateMachine();

            BindMainMenuController();
            BindInGameUIController();
            BindCameraRotation();

            BindPlayerRating();
            BindRatingSaver();

            Container.BindInterfacesAndSelfTo<InGameRatingListUI>().FromInstance(_ratingListUI).AsSingle();
            Container.BindInterfacesAndSelfTo<ReferrerClicks>().AsSingle();
            
            Container.Bind<IMongoConnection>().FromInstance(ValuesFromBootScene.MongoConnection).AsSingle()
                .NonLazy(); 
            
            Container.Bind<IDBValues>().FromInstance(ValuesFromBootScene.DBValues).AsSingle()
                .NonLazy();
            
            
        }

        private void BindRatingSaver()
        {
            Container.BindInterfacesAndSelfTo<AllClickSaver>().AsSingle();
        }

        private void BindPlayerRating()
        {
            Container.BindInterfacesAndSelfTo<PlayerRatingInDBSaver>().FromInstance(_playerRatingInDBSaver).AsSingle();
        }

        private void BindCameraRotation()
        {
            Container.BindInterfacesTo<CameraRotationObject>().FromInstance(_cameraRotation).AsSingle();
        }

        private void BindInGameUIController()
        {
            Container.BindInterfacesTo<InGameUIController>().FromInstance(_inGameUIController).AsSingle();
        }

        private void BindMainMenuController()
        {
            Container.BindInterfacesTo<MainMenuController>().FromInstance(_mainMenuController).AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.BindInterfacesTo<GameStateMachine>().FromInstance(_gameStateMachine).AsSingle();
        }

        private void BindLocationMainMenuFactory()
        {
            Container.BindInterfacesAndSelfTo<LocationMainMenuFactory>().AsSingle();
        }

        private void BindUIHealthBar()
        {
            Container.Bind<IHealthBarUIService>().FromInstance(_healthBarUIController).AsSingle();
        }

        private void BindStoneEventClick()
        {
            Container.BindInterfacesAndSelfTo<StoneEventController>().AsSingle();
        }

        private void BindLocationFactory()
        {
            Container.BindInterfacesAndSelfTo<LocationsObjectFactory>().AsSingle();
        }

        private void BindLocationDataHolder()
        {
            Container.BindInterfacesTo<LocationsObjectDataHolder>().FromInstance(_locationsObjectDataHolder).AsSingle();
        }

        private void BindNetworkManager()
        {
            Container.Bind<INetworkManagerService>().FromInstance(ValuesFromBootScene.CustomNetworkManager).AsSingle()
                .NonLazy();
        }

        private void BindController()
        {
            Container.Bind<IInputEvents>().FromInstance(_inputController).AsSingle();
        }
    }
}