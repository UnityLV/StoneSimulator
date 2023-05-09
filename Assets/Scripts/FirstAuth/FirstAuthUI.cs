using System;
using System.Collections.Generic;
using Network.Enum;
using Network.Interfaces;
using PlayerData.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace FirstAuth
{
    public class FirstAuthUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _inputField;

        [SerializeField]
        private GameObject _popup;

        private bool _isPopupShowed
        {
            get => PlayerPrefs.GetInt("IsPopupShowed", 0) == 1;
            set => PlayerPrefs.SetInt("IsPopupShowed", value ? 1 : 0);
        }

        #region Dependency

        private INicknameDataService _nicknameDataService;
        private INetworkManagerService _networkManagerService;

        [Inject]
        private void Construct(
            INicknameDataService nicknameDataService,
            INetworkManagerService networkManagerService)
        {
            _nicknameDataService = nicknameDataService;
            _networkManagerService = networkManagerService;
        }

        #endregion


        private void Start()
        {
         if ( _networkManagerService.GetConnectionType()==ConnectionType.Server) return;
            _popup.SetActive(!_isPopupShowed);
        }

        public void EndEditNickname()
        {
            if (string.IsNullOrWhiteSpace(_inputField.text)) return;
            string result = _inputField.text;
            _nicknameDataService.SetNickname(result);
            Debug.Log($"User set nick name \"{_nicknameDataService.GetNickname()}\"");
        }

        public void ChangeStateIsShowedPopup(bool state)
        {
            _isPopupShowed = state;
        }
    }
}