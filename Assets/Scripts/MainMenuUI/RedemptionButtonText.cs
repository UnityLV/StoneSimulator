using System;
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
        private int _cost;
        private void Awake()
        {
            LocalizationManager.OnLocalizeEvent += OnUpdateLanguage;
            _slaveRedeemedSystem.CostSeted += SetCost;
        }

        private void OnDestroy()
        {
            LocalizationManager.OnLocalizeEvent -= OnUpdateLanguage;
            _slaveRedeemedSystem.CostSeted -= SetCost;
        }

        private void OnUpdateLanguage()
        {
            SetText();
        }

        public void SetCost(int cost)
        {
            _cost = cost;
            SetText();
        }

        private void SetText()
        {
            if (LocalizationManager.CurrentLanguage == "English")
            {
                _buttonNext.text = $"REDEEM\n{_cost} clicks";
            }

            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                _buttonNext.text = $"ВЫКУПИТЬСЯ\n{_cost} кликов";
            }
        }
    }
}