using System;
using System.Threading.Tasks;
using MongoDBCustom;
using UnityEngine;
using Zenject;
namespace ChatDB.PinMessage
{
    public class CurrentPinMessage : MonoBehaviour
    {
        [SerializeField] private ChatMessageGameObject _message;
        
        
        private IDBCommands _idbCommands;

        [Inject]
        public void Construct(IDBCommands idbCommands)
        {
            _idbCommands = idbCommands;
        }

        private void Start()
        {
            GetCurrentPinMessage();
        }
        
        public async void UpdatePinMessage()
        {
            await Task.Delay(1000);
            GetCurrentPinMessage();
        }

        private async void GetCurrentPinMessage()
        {
            PinMessageData message = await _idbCommands.GetPinnedMessageAsync();

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