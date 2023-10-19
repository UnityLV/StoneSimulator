using CameraRotation;
using GameScene;
using GameState;
using Health;
using Health.Interfaces;
using InGameUI;
using UnityEngine;
using Zenject;
using InputService;
using InputService.Interfaces;
using LocationGameObjects;
using MainMenuUI;
using MainMenuUI.LocationMainMenu;
using MongoDBCustom;
using Network.Interfaces;
using PlayerData;
using Stone;
using UnityEngine.Serialization;

namespace Installers
{
    public class GameSceneLocationInstaller : MonoInstaller
    {
        [SerializeField] private AbilityClickEffectDataHolder _abilityClickEffectDataHolder;
        [SerializeField] private InputController _inputController;

        [SerializeField] private LocationsObjectDataHolder _locationsObjectDataHolder;

        [SerializeField] private HealthBarUIController _healthBarUIController;

        [SerializeField] private GameStateMachine _gameStateMachine;

        [SerializeField] private InGameUIController _inGameUIController;

        [SerializeField] private MainMenuController _mainMenuController;

        [SerializeField] private CameraRotationObject _cameraRotation;
        [SerializeField] private InGameRatingListUI _ratingListUI;

        [SerializeField] private RegularPlayerRatingInDBSaver _regularPlayerRatingInDBSaver;
        [SerializeField] private RankData _rankData;
        [FormerlySerializedAs("_abilityButton")] [SerializeField] private AbilityClicks _abilityClicks;



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
            Container.BindInterfacesAndSelfTo<AbilityClicks>().FromInstance(_abilityClicks).AsSingle();
            Container.BindInterfacesAndSelfTo<ReferrerClicks>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlaveClicksCollector>().AsSingle();
            Container.Bind<IMongoConnection>().FromInstance(ValuesFromBootScene.MongoConnection).AsSingle().NonLazy();
            Container.Bind<IDBCommands>().FromInstance(ValuesFromBootScene.IdbCommands).AsSingle().NonLazy();
            Container.Bind<RankData>().FromInstance(_rankData);
            Container.BindInterfacesAndSelfTo<PlayerRank>().AsSingle();
            Container.BindInterfacesAndSelfTo<ConstantLocationEffects>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClickOnStoneEffects>().AsSingle();
            Container.BindInterfacesAndSelfTo<AbilityClickEffect>().AsSingle();
            Container.BindInterfacesAndSelfTo<AbilityClickEffectDataHolder>().FromInstance(_abilityClickEffectDataHolder).AsSingle();
            
        }

        private void BindRatingSaver()
        {
            Container.BindInterfacesAndSelfTo<AllClickSaver>().AsSingle();
        }

        private void BindPlayerRating()
        {
            Container.BindInterfacesAndSelfTo<RegularPlayerRatingInDBSaver>().FromInstance(_regularPlayerRatingInDBSaver).AsSingle();
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