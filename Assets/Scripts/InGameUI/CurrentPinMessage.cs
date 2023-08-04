using System;
using System.Collections.Generic;
using MongoDBCustom;
using UnityEngine;
using Zenject;
namespace ChatDB.PinMessage
{
    public class CurrentPinMessage : MonoBehaviour
    {
        [SerializeField] private ChatMessageGameObject _message;
        
        
        private IDBValues _dbValues;

        [Inject]
        public void Construct(IDBValues dbValues)
        {
            _dbValues = dbValues;
        }

        private void Start()
        {
            GetCurrentPinMessage();
        }

        private async void GetCurrentPinMessage()
        {
            PinMessageData message = await _dbValues.GetPinnedMessageAsync();

            bool isMessageExist = message.PinDate != DateTime.MinValue;    
            if (isMessageExist)
            {
                _message.gameObject.SetActive(true);
                _message.NicknameText.text = message.Message.PlayerNickname;
                _message.MessageText.text = message.Message.MessageText;
                
            }
        }

       
    }
}