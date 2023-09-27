using I2.Loc;
using UnityEngine;

namespace MainMenuUI
{
    public class LocalizationObjectSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _russianText;
        [SerializeField] private GameObject _englishText;

        private void OnEnable()
        {
            LocalizationManager.OnLocalizeEvent += UpdateText;
            UpdateText();
        }

        private void OnDisable()
        {
            LocalizationManager.OnLocalizeEvent -= UpdateText;
        }

        private void UpdateText()
        {
            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                _englishText.SetActive(false);
                _russianText.SetActive(true);
                return;
            }

            _englishText.SetActive(true);
            _russianText.SetActive(false);
        }
    }
}