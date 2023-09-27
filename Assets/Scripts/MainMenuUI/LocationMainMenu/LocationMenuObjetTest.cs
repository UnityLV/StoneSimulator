using NaughtyAttributes;
using UnityEngine;

namespace MainMenuUI.LocationMainMenu
{
    public class LocationMenuObjetTest : MonoBehaviour
    {
        [SerializeField] private LocationMainMenuObject _locationMain;
        [SerializeField] private LocationMainMenuController _locationMainMenuController;

        [SerializeField] private int _locationIndex;

        [Button]
        public void Set()
        {
            _locationMainMenuController.UpdateLocationInternalState(_locationMain, _locationIndex);
        }
    }
}