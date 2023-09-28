using System.Collections.Generic;
using I2.Loc;

namespace MainMenuUI
{
    public static class LocationNames
    {
        private static Dictionary<int, string> _locationNamesRU = new()
        {
            [-1] = "Локация",
            [0] = "Локация: Первая локация",
            [1] = "Локация: Вторая локация",
            [2] = "Локация: Третья локация",
        };

        private static Dictionary<int, string> _locationNamesEN = new()
        {
            [-1] = "Location",
            [0] = "Location: First location",
            [1] = "Location: Second location",
            [2] = "Location: Third location",
        };

        public static string GetLocationName(int locationIndex)
        {
            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                return GetKeyOrDefault(_locationNamesRU, locationIndex);
            }

            return GetKeyOrDefault(_locationNamesEN, locationIndex); 
        }

        private static K GetKeyOrDefault<K>(Dictionary<int, K> dict, int key)
        {
            if (dict.TryGetValue(key, out K value))
            {
                return value;
            }

            return dict[-1];
        }
    }
}