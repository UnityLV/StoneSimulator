using TMPro;
using UnityEngine;

namespace InGameUI
{
    public class InMenuRatingListUI : RatingListUI
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private TMP_Text _playerRaitingPosition;
        [SerializeField] private TMP_Text _playerRaitingPosition2;
        [SerializeField] private TMP_Text _allClickCount;
     
        protected override void OnSetData()
        {
            _playerName.text = NicknameData.GetNickname();
            _allClickCount.text = ClickData.GetAllClickCount().ToString();
        }

        //TODO: formating numbers from 10000 to 10.000
        protected override void OnFindPlayerInRating(int rating)
        {
            _playerRaitingPosition.text = rating.ToString();
            _playerRaitingPosition2.text = rating.ToString();
        }
    }
}