using FirebaseCustom;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace MainMenuUI
{
    public class ADForClicksText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        PlayerConfig PlayerConfig = RemoteConfigSetter.PlayerConfig;
        
        private void Awake()
        {
            LocalizationManager.OnLocalizeEvent += OnUpdateLanguage;
            SetText();
        }

        private void OnDestroy()
        {
            LocalizationManager.OnLocalizeEvent -= OnUpdateLanguage;
        }

        private void OnUpdateLanguage()
        {
            SetText();
        }

        private void SetText()
        {
            int clicks = PlayerConfig.ClicksFromAD;

            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                
                _text.text = $"Получите {clicks} кликов за просмотр рекламы";
                return;
            }
            _text.text = $"Get {clicks} clicks for viewing ads";
        }
    }
}