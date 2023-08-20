using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBCustom;
using Network.Enum;
using Network.Interfaces;
using PlayerData.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using Zenject;

namespace FirstAuth
{
    public class FirstAuthUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;

        [SerializeField] private GameObject _popup;
        [SerializeField] private Button _confirmNickButton;


        public bool IsPopupShowed
        {
            get => PlayerPrefs.GetInt("IsPopupShowed", 0) == 1;
            private set => PlayerPrefs.SetInt("IsPopupShowed", value ? 1 : 0);
        }

        private bool IsNicknameEmpty => _nicknameDataService.GetNickname() == String.Empty;

        #region Dependency

        private IDBCommands _idbCommands;
        private INicknameDataService _nicknameDataService;
        private INetworkManagerService _networkManagerService;

        [Inject]
        private void Construct(
            INicknameDataService nicknameDataService,
            INetworkManagerService networkManagerService, IDBCommands idbCommands)
        {
            _nicknameDataService = nicknameDataService;
            _networkManagerService = networkManagerService;
            _idbCommands = idbCommands;
        }

        #endregion

        private void Start()
        {
            if (_networkManagerService == null)
            {
                return;
            }

            if (_networkManagerService.GetConnectionType() == ConnectionType.Server) return;
            _popup.SetActive(IsNicknameEmpty);

            _confirmNickButton.interactable = false;
        }

        public void OnNickChanged(string nick)
        {
            if (IsValidNick(nick))
            {
                ProcessValidNick();
            }
            else
            {
                ProcessInvalidNick();
            }
        }

        private void ProcessInvalidNick()
        {
            _confirmNickButton.interactable = false;
        }

        private void ProcessValidNick()
        {
            _confirmNickButton.interactable = true;
        }

        public void EndEditNickname()
        {
            string result = _inputField.text;

            if (IsValidNick(result) == false) return;
            _nicknameDataService.SetNickname(result);
            Debug.Log($"User set nick name \"{_nicknameDataService.GetNickname()}\"");

            _idbCommands.UpdatePlayerName(result);
        }

        private bool IsValidNick(string nick)
        {
            return string.IsNullOrWhiteSpace(nick) == false;
        }

        public void ChangeStateIsShowedPopup(bool state)
        {
            IsPopupShowed = state;
        }
    }
}