using TMPro;
using UnityEngine;

namespace InGameUI
{
    public class InGameRatingListUI : RatingListUI
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private TMP_Text _playerRaitingPosition;
        [SerializeField] private TMP_Text _clickCount;
     
        protected override void OnSetData()
        {
            _playerName.text = NicknameData.GetNickname();
            _clickCount.text = $"Клики: { ClickData.GetAllClickCount().ToString()}";
        }

        protected override void OnFindPlayerInRating(int raiting)
        {
            _playerRaitingPosition.text = raiting.ToString();
        }
    }
}