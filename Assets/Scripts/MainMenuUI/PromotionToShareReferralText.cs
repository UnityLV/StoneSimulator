using FirebaseCustom;
using I2.Loc;
using TMPro;
using UnityEngine;
namespace MainMenuUI
{
    public class PromotionToShareReferralText : MonoBehaviour
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
            int percentToAdd = PlayerConfig.PercentToAddToReferrer;

            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                _text.text = $"Теперь вы можете приглашать друзей и получать {percentToAdd}% их кликов";
                return;
            }

            _text.text = $"Now you can invite friends and get {percentToAdd}% of their clicks";
        }
    }
}