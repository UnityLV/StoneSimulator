using GameState.Interfaces;
using I2.Loc;
using LocationGameObjects.Interfaces;
using UnityEngine;
using Zenject;

namespace MainMenuUI
{
    public class LocationWindowDataConstructor : MonoBehaviour
    {
        [Inject] private IGetLocationSpritesService _getLocationSpritesService;
        [Inject] private IHealthService _healthService;

        public void SetDataFor(CompleteLocationWindow window, int level)
        {
            window.SetData(ConstructData(level));
        }

        private LocationWindowData ConstructData(int level)
        {
            return new LocationWindowData(
                _getLocationSpritesService.GetAvatarLocationSprite(level),
                (level + 1),
                LocationNames.GetLocationName(level),
                CalculateClicks(level)
            );
        }

        private string CalculateClicks(int level)
        {
            int allClickInLocation = GetAllClicksInLocation(level);
            return $"{allClickInLocation} / {allClickInLocation} {GetClicksName()}.";
        }

        private int GetAllClicksInLocation(int level)
        {
           return _healthService.GetAllHealthInLocation(level);
        }

        private string GetClicksName()
        {
            return LocalizationManager.CurrentLanguage == "Russian" ? "Кликов" : "Clicks";
        }
    }
}