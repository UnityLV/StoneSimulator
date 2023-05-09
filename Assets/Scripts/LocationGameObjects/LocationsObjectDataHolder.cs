using System;
using System.Collections.Generic;
using LocationGameObjects.Interfaces;
using Mirror;
using UnityEngine;

namespace LocationGameObjects
{
    public class LocationsObjectDataHolder : NetworkBehaviour, IGetLocationGameObjectService, IGetLocationCountService, IGetLocationSpritesService
    {
        [SerializeField]
        private List<Locations> _locationsLists = new List<Locations>();
        //private readonly SyncList<Locations> _locationsSyncLists = new SyncList<Locations>();
        
        public List<GameObject> GetLocationsGameObject(int location)
        {
            return  _locationsLists[location].BgObjects;
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

            [SerializeField]
            public Sprite BGLocationSprite;
            
            [SerializeField]
            public Sprite AvatarLocationSprite;

        }

        public Sprite GetBGLocationSprite(int location)
        {
            return _locationsLists[location].BGLocationSprite;
        }

        public Sprite GetAvatarLocationSprite(int location)
        {
            return _locationsLists[location].AvatarLocationSprite;
        }
    }
}