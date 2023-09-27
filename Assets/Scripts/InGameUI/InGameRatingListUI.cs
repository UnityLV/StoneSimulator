using I2.Loc;
using TMPro;
using UnityEngine;

namespace InGameUI
{
    public class InGameRatingListUI : RatingListUI
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private TMP_Text _playerRaitingPosition;
        [SerializeField] private TMP_Text _clickCount;
        private int Count => ClickData.GetAllClickCount();

        private void OnEnable()
        {
            LocalizationManager.OnLocalizeEvent += UpdateText;
        }

        private void UpdateText()
        {
            if (LocalizationManager.CurrentLanguage == "Russian")
            {
                _clickCount.text = $"Кликов: {Count.ToString()}";
                return;
            }

            _clickCount.text = $"Clicks: {Count.ToString()}";
        }

        private void OnDisable()
        {
            LocalizationManager.OnLocalizeEvent -= UpdateText;
        }

        protected override void OnSetData()
        {
            _playerName.text = NicknameData.GetNickname();
            UpdateText();
        }

        protected override void OnFindPlayerInRating(int rating)
        {
            _playerRaitingPosition.text = rating.ToString();
        }
    }
}