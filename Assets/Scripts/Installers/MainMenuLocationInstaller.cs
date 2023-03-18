using Network.Interfaces;
using Zenject;

namespace Installers
{
    public class MainMenuLocationInstaller : MonoInstaller
    {
    
        public override void InstallBindings()
        {
            BindNetworkCallbacks();
            BindNetworkManager();
        }

        private void BindNetworkCallbacks()
        {
            Container.Bind<INetworkCallbacks>().FromInstance(BootSceneInstaller.NetworkCallbackObject).AsSingle().NonLazy();
        }
        
        private void BindNetworkManager()
        {
            Container.Bind<INetworkManagerService>().FromInstance(BootSceneInstaller.CustomNetworkManager).AsSingle().NonLazy();
        }
    }
    
}