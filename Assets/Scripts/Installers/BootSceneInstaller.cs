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

        public static CustomNetworkManager CustomNetworkManager;

        public override void InstallBindings()
        {
            BindNetworkManager();
        }

        private void BindNetworkManager()
        {
            Container.Bind<INetworkManagerService>().FromInstance(_customNetworkManager).AsSingle().NonLazy();
            CustomNetworkManager = _customNetworkManager;
        }
    }
}