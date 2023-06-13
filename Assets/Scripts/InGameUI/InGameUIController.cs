using System;
using System.Collections.Generic;
using I2.Loc;
using InGameUI.Interfaces;
using PlayerData.Interfaces;
using Stone.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace InGameUI
{
    public class InGameUIController : MonoBehaviour, IInGameService
    {
        [SerializeField]
        private GameObject _inGameUIMain;

        [SerializeField]
        private List<GameObject> _mainPlayObjects;

        [SerializeField]
        private TextMeshProUGUI _nickname;

        [SerializeField]
        private LocalizationParamsManager _clickCountParam;

        private Action _onHomeAction;

        #region Dependency

        private IClickDataService _clickDataService;
        private INicknameDataService _nicknameDataService;

        [Inject]
        private void Construct(IClickDataService clickDataService,
            INicknameDataService nicknameDataService)
        {
            _clickDataService = clickDataService;
            _clickDataService.ClickUpdated += UpdateClickCount;
            _nicknameDataService = nicknameDataService;
        }

        #endregion


        public void SetState(bool mainState, bool playState)
        {
            _inGameUIMain.SetActive(mainState);
            if (mainState)
            {
                UpdateClickCount(_clickDataService.GetClickCount());
                _nickname.text = _nicknameDataService.GetNickname(); 
            }

            foreach (var VARIABLE in _mainPlayObjects)
            {
                VARIABLE.SetActive(playState);
            }
        }

        public void SetOnHomeClickAction(Action action)
        {
            _onHomeAction = action;
        }

        public void OnHomeClick()
        {
            _onHomeAction?.Invoke();
        }

        public void UpdateClickCount(int amount)
        {
            _clickCountParam.SetParameterValue("CLICKS", amount.ToString());
        }
    }
}