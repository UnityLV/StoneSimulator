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
        Container.BindInterfacesAndSelfTo<PlayerDataHolder>().AsSingle();
    }

    private void BindGameScene()
    {
        Container.BindInterfacesAndSelfTo<GameSceneController>().AsSingle();
    }
}