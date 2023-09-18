using System;
using AYellowpaper;
using MongoDBCustom;
using ChatDB.PinMessage;
using GameScene;
using PlayerData.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
namespace InGameUI
{
    public class Market : MonoBehaviour
    {
        [SerializeField] private UnityEvent _pinMessageConfirm;
        [SerializeField] private InterfaceReference<IMarketLogic> _marketLogic;
        [SerializeField] private UIMoveButton _closeButton;

        [SerializeField] private GameObject _shopPanel;
        

        private IDBCommands _idbCommands;

        [SerializeField] private PinMessageSystemAndUI _pinMessageSystemAndUI;
        [SerializeField] private AbilityClicks _abilityClicks;

        [Inject]
        private void Construct(IDBCommands idbCommands)
        {
            _idbCommands = idbCommands;
        }

        private void Awake()
        {
            _pinMessageSystemAndUI.SendingDataConstructed += OnPinSendingDataConstructed;
        }
        
        private void OnDestroy()
        {
            _pinMessageSystemAndUI.SendingDataConstructed -= OnPinSendingDataConstructed;
        }

        public void BuyClicks()
        {
            _marketLogic.Value.BuyClicks(OnConfirmBuyClick);
        }

        public void RemoveAD()
        {
            _marketLogic.Value.BuyRemoveAD(OnConfirmRemoveAD);
        }

        public void PinMessage()
        {
            HideShopPanelButLeaveBackground();
            _pinMessageSystemAndUI.StartListenPlayerInputForPinMessage();
        }
        
        private void HideShopPanelButLeaveBackground()
        {
            _shopPanel.SetActive(false);
        }

        public void CrossButtonOnMainPinMessagePanel()
        {
            HidePanelByNormal();
        }

        private async void HidePanelByNormal()
        {
            await _closeButton.AsyncMove();
            _shopPanel.SetActive(true);
        }

        private void OnPinSendingDataConstructed(PinMessageData data)
        {
            _marketLogic.Value.BuyPinMessage(() => OnConfirmPinMessage(data));
            HidePanelByNormal();
        }

        private void OnConfirmBuyClick()
        {
            int add = 2500;
            _abilityClicks .AddClicks(add);
            Debug.Log("Buy Clicks");
        }

        private void OnConfirmRemoveAD()
        {
            Debug.Log("Remove Ad");
        }

        private void OnConfirmPinMessage(PinMessageData data)
        {
            _pinMessageConfirm?.Invoke();
            _idbCommands.PinMessageAsync(data);
            Debug.Log("Pin Message " + data);
        }
    }
}