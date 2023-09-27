using FirebaseCustom;
using I2.Loc;
using TMPro;
using UnityEngine;
namespace MainMenuUI
{
    public class PromotionToShareReferralAndGetTotalText : MonoBehaviour
    {
        [SerializeField] private string _prefixRu;
        [SerializeField] private string _prefixEn;
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
            int clicks = PlayerConfig.EarnedFromEachReferral;
            int percentToAdd = PlayerConfig.PercentToAddToReferrer;
            
            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                _text.text = $"{_prefixRu}\nПолучайте {clicks} кликов и {percentToAdd}% от заработка с каждого приглашённого друга";
                return;
            }
            _text.text = $"{_prefixEn}\nGet {clicks} clicks and {percentToAdd}% of earnings from each invited friend";
        }
    }
}