using MongoDBCustom;
using Network;
using Network.Interfaces;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BootSceneInstaller:MonoInstaller
    {
        [SerializeField]
        private CustomNetworkManager _customNetworkManager;

        [SerializeField] private MongoDBProvider _mongoDBProvider;

        [SerializeField] private MongoDBConnectionDataHolder _mongoDBConnectionDataHolder;


        public static CustomNetworkManager CustomNetworkManager;

        public override void InstallBindings()
        {
            BindNetworkManager();
            
            Container.Bind<IDBProvider>().FromInstance(_mongoDBProvider).AsSingle().NonLazy();
            Container.Bind<MongoDBConnectionDataHolder>().FromInstance(_mongoDBConnectionDataHolder).AsSingle().NonLazy();
        }

        private void BindNetworkManager()
        {
            Container.Bind<INetworkManagerService>().FromInstance(_customNetworkManager).AsSingle().NonLazy();
            CustomNetworkManager = _customNetworkManager;
        }
    }
}