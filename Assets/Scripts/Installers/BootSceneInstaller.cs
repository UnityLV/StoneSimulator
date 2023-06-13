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

        [SerializeField] private MongoDBDataHolder _mongoDBDataHolder;


        public static CustomNetworkManager CustomNetworkManager;

        public override void InstallBindings()
        {
            BindNetworkManager();
            
            Container.Bind<IDBProvider>().FromInstance(_mongoDBProvider).AsSingle().NonLazy();
            Container.Bind<MongoDBDataHolder>().FromInstance(_mongoDBDataHolder).AsSingle().NonLazy();
        }

        private void BindNetworkManager()
        {
            Container.Bind<INetworkManagerService>().FromInstance(_customNetworkManager).AsSingle().NonLazy();
            CustomNetworkManager = _customNetworkManager;
        }
    }
}