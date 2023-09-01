﻿using System;
using AYellowpaper;
using MongoDBCustom;
using ChatDB.PinMessage;
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
        
        private IClickDataService _clickDataService;
        private IDBAllClickSaver _dbAllClickSaver;

        private IDBCommands _idbCommands;

        [SerializeField] private PinMessageUISystem _pinMessageUISystem;

        [Inject]
        private void Construct(IClickDataService clickDataService, IDBAllClickSaver dbAllClickSaver , IDBCommands idbCommands)
        {
            _clickDataService = clickDataService;
            _dbAllClickSaver = dbAllClickSaver;
            _idbCommands = idbCommands;
        }

        private void OnEnable()
        {
            _pinMessageUISystem.DataConstructed += OnPinDataConstructed;
        }
        
        private void OnDisable()
        {
            _pinMessageUISystem.DataConstructed -= OnPinDataConstructed;
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
            _pinMessageUISystem.StartListenPlayerInputForPinMessage();
            
        }
        
        private void OnPinDataConstructed(PinMessageData data)
        {
            _marketLogic.Value.BuyPinMessage(() => OnConfirmPinMessage(data));
        }

        private void OnConfirmBuyClick()
        {
            int add = 2500;
            _clickDataService.AddClicks(add);
            _dbAllClickSaver.Save(add);

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