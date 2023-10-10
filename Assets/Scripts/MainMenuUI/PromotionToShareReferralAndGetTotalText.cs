using FirebaseCustom;
using I2.Loc;
using TMPro;
using UnityEngine;
namespace MainMenuUI
{
    public class PromotionToShareReferralAndGetTotalText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private PlayerConfig PlayerConfig = RemoteConfigSetter.PlayerConfig;

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
            int clicks = PlayerConfig.EarnedFromEachReferral;
            int percentToAdd = PlayerConfig.PercentToAddToReferrer;
            
            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                _text.text = $"Получайте {clicks} кликов и {percentToAdd}% от заработка с каждого приглашённого друга";
                return;
            }
            _text.text = $"Get {clicks} clicks and {percentToAdd}% of earnings from each invited friend";
        }
    }
}