using FirebaseCustom;
using I2.Loc;
using TMPro;
using UnityEngine;
namespace MainMenuUI
{
    public class SlavePayRoReferralText : MonoBehaviour
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
            if (PlayerConfig is null)
            {
                return;
            }
            int percentToAdd = PlayerConfig.PercentToAddToReferrer;

            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                _text.text = $"Он зарабатывает на вас {percentToAdd}% с ваших кликов";
                return;
            }

            _text.text = $"He earns {percentToAdd}% from your clicks";
        }
    }
}