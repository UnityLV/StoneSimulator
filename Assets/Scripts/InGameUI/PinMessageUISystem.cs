using System;
using UnityEngine;
using UnityEngine.Events;
namespace ChatDB.PinMessage
{
    public class PinMessageUISystem : MonoBehaviour
    {
        [SerializeField] private PinMessageCalendar _messageCalendar;
        [SerializeField] private PinMessageTextInput _messageTextInput;
        [SerializeField] private ChatDB _chatDB;
        
        private DateTime _selectedDate;
        private string _message;

        public event UnityAction<PinMessageData> DataConstructed;

        private void OnEnable()
        {
            _messageCalendar.DateSelected += OnDateSelected;
            _messageTextInput.GetMessage += OnGetMessage;
        }


        private void OnDisable()
        {
            _messageCalendar.DateSelected -= OnDateSelected;
            _messageTextInput.GetMessage -= OnGetMessage;
        }

        public void StartListenPlayerInputForPinMessage()
        {
            _messageCalendar.Show();
        }

        private void OnDateSelected(DateTime date)
        {
            _selectedDate = date;
            _messageCalendar.Hide();
            _messageTextInput.Show();
        }

        private void OnGetMessage(string message)
        {
            _message = message;
            _messageTextInput.Hide();

            DataConstructed?.Invoke(new PinMessageData { Message = _chatDB.ConvertToChatMessage(_message) , PinDate = _selectedDate });
        }
    }
}