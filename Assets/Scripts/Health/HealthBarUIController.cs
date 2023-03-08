using Health.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class HealthBarUIController: MonoBehaviour, IHealthBarUIService
    {
        [SerializeField]
        private Image _healthbarProgressImage;
        
        [SerializeField]
        private TextMeshProUGUI _healthbarProgressText;
        
        public void UpdateHealthBarState(int currentHp, int maxHp)
        {
            _healthbarProgressImage.fillAmount=currentHp / (float) maxHp;
            _healthbarProgressText.text = $"{currentHp}/{maxHp}";
        }
    }
}