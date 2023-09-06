using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
namespace ChatDB.PinMessage
{
    public class PinMessageSystemAndUI : MonoBehaviour //TODO: Выделить UI в обдельный класс
    {
        [SerializeField] private PinMessageCalendar _messageCalendar;
        [SerializeField] private CalendarDateCellColorizer _calendarDateCellColorizer;
        [SerializeField] private Calendar _calendar;
        [SerializeField] private ChatDB _chatDB;
        [SerializeField] private GameObject _mainInputWindow;
        [SerializeField] private TMP_Text _selectedDateTextsInCalendar;
        [SerializeField] private TMP_Text _selectedDateTextsInMainWindow;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private CalendarInput _calendarInput;

        private DateTime _selectedDate;
        public event UnityAction<PinMessageData> SendingDataConstructed;

        private bool _inWork;


        private void Awake()
        {
            _messageCalendar.DateSelected += OnDateSelected;
        }

        private void OnDestroy()
        {
            _messageCalendar.DateSelected -= OnDateSelected;
        }

        public async void SendPinMessageButton()
        {
            if (_inWork)
                return;
            _inWork = true;

            if (await IsDateValid() == false)
            {
                _inWork = false;
                return;
            }
            if (IsMessageExist(out string message) == false)
            {
                _inWork = false;
                return;
            }

            ConstructMessage(message);

            _inWork = false;
        }

        private async Task<bool> IsDateValid()
        {
            return _selectedDate != default && await IsDateAvailableForPin(_selectedDate);
        }

        private bool IsMessageExist(out string message)
        {
            message = _inputField.text;
            return string.IsNullOrWhiteSpace(_inputField.text) == false;
        }

        private async Task<bool> IsDateAvailableForPin(DateTime date)
        {
            return await _chatDB.IsDateNotHavePinMessage(date);
        }

        public void StartListenPlayerInputForPinMessage()
        {
            EnableMainInputWindow();
        }

        private void EnableMainInputWindow()
        {
            _mainInputWindow.SetActive(true);
        }

        private void DisableWindows()
        {
            _mainInputWindow.SetActive(false);
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
            _selectedDateTextsInCalendar.text = DateTools.ConvertToCustomFormat(_selectedDate);
        }

        private void ConstructMessage(string message)
        {
            PinMessageData pinMessageData = new() { Message = _chatDB.ConvertToChatMessage(message), PinDate = _selectedDate };
            SendingDataConstructed?.Invoke(pinMessageData);
            DisableWindows();
            ResetCalendar();
        }

        private void ResetCalendar()
        {
            _selectedDateTextsInCalendar.text = "NOT SELECTED";
            _selectedDateTextsInMainWindow.text = "NOT SELECTED";
            _inputField.text = String.Empty;
            _calendarInput.ResetInput();
        }
    }
}