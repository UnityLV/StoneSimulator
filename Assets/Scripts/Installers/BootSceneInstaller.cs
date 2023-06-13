using MongoDBCustom;
using Network;
using Network.Interfaces;
using PlayerData;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BootSceneInstaller : MonoInstaller
    {
        [SerializeField] private CustomNetworkManager _customNetworkManager;

        [SerializeField] private MongoDBConnectionDataProvider _mongoDBConnectionDataProvider;
        [SerializeField] private MongoDBConnectionDataHolder _mongoDBConnectionDataHolder;

        [SerializeField] private MongoDBPlayerDataProvider _dbPlayerDataProvider;
        [SerializeField] private MongoDBPlayerDataHolder _mongoDBPlayerData;


        public static CustomNetworkManager CustomNetworkManager;

        public override void InstallBindings()
        {
            BindNetworkManager();
            BindConnectionProvider();
            BindPlayerDataProvider();
            BindMongoDBPlayerDataHolder();
            BindConnectionDataHolder();
        }

        private void BindConnectionDataHolder()
        {
            Container.Bind<MongoDBConnectionDataHolder>().FromInstance(_mongoDBConnectionDataHolder).AsSingle()
                .NonLazy();
        }

        private void BindMongoDBPlayerDataHolder()
        {
            Container.BindInterfacesAndSelfTo<MongoDBPlayerDataHolder>().FromInstance(_mongoDBPlayerData).AsSingle()
                .NonLazy();
        }

        private void BindPlayerDataProvider()
        {
            Container.BindInterfacesAndSelfTo<IDBPlayerDataProvider>().FromInstance(_dbPlayerDataProvider).AsSingle()
                .NonLazy();
        }

        private void BindConnectionProvider()
        {
            Container.Bind<IDBConnectionProvider>().FromInstance(_mongoDBConnectionDataProvider).AsSingle();
        }

        private void BindNetworkManager()
        {
            Container.Bind<INetworkManagerService>().FromInstance(_customNetworkManager).AsSingle().NonLazy();
            CustomNetworkManager = _customNetworkManager;
        }
    }
}