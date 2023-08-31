using System;
using PlayerData.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MainMenuUI
{
    public class PlayerAvatarUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nickname;
        [SerializeField] private TMP_Text _rank;
        [SerializeField] private TMP_Text _whoYouReferrer;

        private INicknameDataService _nicknameData;
        private IRankDataService _rankData;

        [Inject]
        public void Construct(INicknameDataService nicknameData, IRankDataService rankData)
        {
            _nicknameData = nicknameData;
            _rankData = rankData;
        }

        public void UpdateText()
        {
            _nickname.text = _nicknameData.GetNickname();
            _rank.text = _rankData.GetMyRank();
        }
    }
}