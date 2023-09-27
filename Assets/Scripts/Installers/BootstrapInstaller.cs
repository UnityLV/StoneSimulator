using GameScene;
using PlayerData;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{ 
    public override void InstallBindings()
    {
        BindGameScene();
        BindPlayerDataHolder();
    }

    private void BindPlayerDataHolder()
    {

       
        Container.BindInterfacesAndSelfTo<PlayerClickData>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerNicknameData>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerSlavesData>().AsSingle();
        
    }

    private void BindGameScene()
    {
        Container.BindInterfacesAndSelfTo<GameSceneController>().AsSingle();
    }
}