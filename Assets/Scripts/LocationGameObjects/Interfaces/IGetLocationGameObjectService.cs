using System.Collections.Generic;
using UnityEngine;

namespace LocationGameObjects.Interfaces
{
    public interface IGetLocationGameObjectService
    {
        public List<GameObject> GetLocationsGameObject(int location);
        public GameObject GetStoneGameObject(int location, int stoneLvl);
    }
}