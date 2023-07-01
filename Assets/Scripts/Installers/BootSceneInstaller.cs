using MainMenuUI;
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
 
        public override void InstallBindings()
        {
            BindNetworkManager();
            Container.BindInterfacesAndSelfTo<MongoDBPlayerDataProvider>().AsSingle().NonLazy();
         
        }


        private void BindNetworkManager()
        {
            Container.Bind<INetworkManagerService>().FromInstance(_customNetworkManager).AsSingle().NonLazy();
            ValuesFromBootScene.CustomNetworkManager = _customNetworkManager;
        }
    }
}