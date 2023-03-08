using System;
using System.Collections.Generic;
using LocationGameObjects.Interfaces;
using UnityEngine;

namespace LocationGameObjects
{
    public class LocationsObjectDataHolder : MonoBehaviour, IGetLocationGameObjectService, IGetLocationCountService
    {
        [SerializeField]
        private List<Locations> _locationsLists;

        public List<GameObject> GetLocationsGameObject(int location)
        {
            return _locationsLists[location].BgObjects;
        }

        public GameObject GetStoneGameObject(int location, int stoneLvl)
        {
            return _locationsLists[location].StoneObjects[stoneLvl];
        }

        public int GetLocationsCount()
        {
            return _locationsLists.Count;
        }

        public int GetStoneCount(int location)
        {
            return _locationsLists[location].StoneObjects.Count;
        }

        [Serializable]
        public class Locations
        {
            [SerializeField]
            public List<GameObject> BgObjects;

            [SerializeField]
            public List<GameObject> StoneObjects;
        }
    }
}