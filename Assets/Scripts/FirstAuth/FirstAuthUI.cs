﻿using System;
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
using Zenject;

namespace FirstAuth
{
    public class FirstAuthUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;

        [SerializeField] private GameObject _popup;

        private IDBValues _dbValues;
        public bool IsPopupShowed
        {
            get => PlayerPrefs.GetInt("IsPopupShowed", 0) == 1;
            private set => PlayerPrefs.SetInt("IsPopupShowed", value ? 1 : 0);
        }

        #region Dependency

        private INicknameDataService _nicknameDataService;
        private INetworkManagerService _networkManagerService;

        [Inject]
        private void Construct(
            INicknameDataService nicknameDataService,
            INetworkManagerService networkManagerService,IDBValues dbValues)
        {
            _nicknameDataService = nicknameDataService;
            _networkManagerService = networkManagerService;
            _dbValues = dbValues;
        }

        #endregion

        private void Start()
        {
            if (_networkManagerService == null)
            {
                return;
            }
            if (_networkManagerService.GetConnectionType() == ConnectionType.Server) return;
            _popup.SetActive(!IsPopupShowed);
        }

        public void EndEditNickname()
        {
            if (string.IsNullOrWhiteSpace(_inputField.text)) return;
            string result = _inputField.text;
            _nicknameDataService.SetNickname(result);
            Debug.Log($"User set nick name \"{_nicknameDataService.GetNickname()}\"");

            _dbValues.UpdatePlayerName(result);
        }


        public void ChangeStateIsShowedPopup(bool state)
        {
            IsPopupShowed = state;
        }
    }
}