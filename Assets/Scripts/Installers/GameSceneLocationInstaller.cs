using Health;
using Health.Interfaces;
using UnityEngine;
using Zenject;
using InputService;
using InputService.Interfaces;
using LocationGameObjects;
using LocationGameObjects.Interfaces;
using Stone;

namespace Installers
{
    public class GameSceneLocationInstaller : MonoInstaller
    {
        [SerializeField]
        private InputController _inputController;

        [SerializeField]
        private LocationsObjectDataHolder _locationsObjectDataHolder;

        [SerializeField]
        private HealthBarUIController _healthBarUIController;
        
       public override void InstallBindings()
       {
           BindController();
           BindLocationDataHolder();
           BindLocationFactory();
           BindStoneEventClick();
           BindUIHealthBar();
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
           Container.Bind<IGetLocationGameObjectService>().FromInstance(_locationsObjectDataHolder).AsSingle();
           Container.Bind<IGetLocationCountService>().FromInstance(_locationsObjectDataHolder).AsSingle();
       }

       private void BindController()
       {
           Container.Bind<IInputEvents>().FromInstance(_inputController).AsSingle();
       }
    }
}