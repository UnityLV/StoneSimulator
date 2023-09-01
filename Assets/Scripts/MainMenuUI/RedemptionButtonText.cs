using System;
using FirebaseCustom;
using I2.Loc;
using InGameUI;
using TMPro;
using UnityEngine;
namespace MainMenuUI
{
    public class RedemptionButtonText : MonoBehaviour
    {
        [SerializeField] private SlaveRedeemedSystem _slaveRedeemedSystem;
        [SerializeField] private TMP_Text _buttonNext;
        [SerializeField] private PlayerConfig _config;

        private void Awake()
        {
            LocalizationManager.OnLocalizeEvent += OnUpdateLanguage;
        }

        private void OnDestroy()
        {
            LocalizationManager.OnLocalizeEvent -= OnUpdateLanguage;
        }

        private void Start()
        {
            SetText();
        }

        private void OnUpdateLanguage()
        {
            SetText();
        }

        private void SetText()
        {
            int cost = _config.ClicksToRedeemed;
            if (LocalizationManager.CurrentLanguage == "English")
            {
                _buttonNext.text = $"REDEEM\n{cost} clicks";
            }

            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                _buttonNext.text = $"ВЫКУПИТЬСЯ\n{cost} кликов";
            }
        }
    }
}