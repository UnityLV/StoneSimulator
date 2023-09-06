using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
namespace ChatDB.PinMessage
{
    public class PinMessageUISystem : MonoBehaviour
    {
        [SerializeField] private PinMessageCalendar _messageCalendar;
        [SerializeField] private CalendarDateCellColorizer _calendarDateCellColorizer;
        [SerializeField] private Calendar _calendar;
        [SerializeField] private PinMessageTextInput _messageTextInput;
        [SerializeField] private ChatDB _chatDB;

        [SerializeField] private GameObject _mainInputWindow;

        [SerializeField] private TMP_Text _selectedDateTextsInCalendar;
        [SerializeField] private TMP_Text _selectedDateTextsInMainWindow;
        private DateTime _selectedDate;
        private string _message;

        public event UnityAction<PinMessageData> DataConstructed;

        private void Awake()
        {
            _messageCalendar.DateSelected += OnDateSelected;
            _messageTextInput.GetMessage += OnGetMessage;
        }

        private void OnDestroy()
        {
            _messageCalendar.DateSelected -= OnDateSelected;
            _messageTextInput.GetMessage -= OnGetMessage;
        }

        public void StartListenPlayerInputForPinMessage()
        {
            EnableMainInputWindow();
        }

        private void EnableMainInputWindow()
        {
            _mainInputWindow.SetActive(true);
        }

        public void EnableCalendar()
        {
            _calendarDateCellColorizer.UpdateDataFromDB();
            _messageCalendar.Show();
            _calendar.SelectCurrentDate();
        }

        public void HideCalendarIfDataSelected()
        {
            if (_selectedDate != default)
            {
                _selectedDateTextsInMainWindow.text = DateTools.ConvertToCustomFormat(_selectedDate);
                _messageCalendar.Hide();
            }
        }

        private void OnDateSelected(SelectedDateButtonData data)
        {
            _selectedDate = data.SelectedDate;
            UpdateYourSelectedDateUI();

            // _messageCalendar.Hide();
            // _messageTextInput.Show();
        }

        private void UpdateYourSelectedDateUI()
        {
            _selectedDateTextsInCalendar.text = DateTools.ConvertToCustomFormat(_selectedDate);
        }

        private void OnGetMessage(string message)
        {
            _message = message;
            _messageTextInput.Hide();

            DataConstructed?.Invoke(new PinMessageData { Message = _chatDB.ConvertToChatMessage(_message), PinDate = _selectedDate });
        }
    }
}