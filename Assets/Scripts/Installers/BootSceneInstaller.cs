using Network;
using Network.Interfaces;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BootSceneInstaller:MonoInstaller
    {
        [SerializeField]
        private NetworkCallbackController _networkCallbackController;

        [SerializeField]
        private CustomNetworkManager _customNetworkManager;

        public static NetworkCallbackController NetworkCallbackObject;
        public static CustomNetworkManager CustomNetworkManager;
        
        public override void InstallBindings()
        {
            BindNetworkCallbacks();
            BindNetworkManager();
        }

        private void BindNetworkManager()
        {
            Container.Bind<INetworkManagerService>().FromInstance(_customNetworkManager).AsSingle().NonLazy();
            CustomNetworkManager = _customNetworkManager;
        }

        private void BindNetworkCallbacks()
        {
            Container.Bind<INetworkCallbacks>().FromInstance(_networkCallbackController).AsSingle().NonLazy();
            NetworkCallbackObject = _networkCallbackController;
        }
    }
}