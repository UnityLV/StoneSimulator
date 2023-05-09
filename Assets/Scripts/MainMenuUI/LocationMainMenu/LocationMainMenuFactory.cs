using System.Collections.Generic;
using MainMenuUI.LocationMainMenu.Interfaces;
using UnityEngine;
using Zenject;

namespace MainMenuUI.LocationMainMenu
{
    public class LocationMainMenuFactory:ILocationFactoryService
    {
        #region Dependency

        private DiContainer _diContainer;

        [Inject]
        public LocationMainMenuFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            Load();
        }

        #endregion


        private const string LOCATION_OBJECT_PATH = "Location";
        private LocationMainMenuObject _locationPrefab;

        private void Load()
        {
            _locationPrefab = Resources.Load<LocationMainMenuObject>(LOCATION_OBJECT_PATH);
        }


        public List<LocationMainMenuObject> CreateLocationObjects(int count)
        {
            List<LocationMainMenuObject> result = new List<LocationMainMenuObject>();
            for (int i = 0; i < count; i++)
            {
                result.Add(_diContainer.InstantiatePrefabForComponent<LocationMainMenuObject>(_locationPrefab));
            }

            return result;
        }
    }
}