using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

        [FormerlySerializedAs("_inputField")] [SerializeField]
        private TMP_InputField _messageInputField;

        [SerializeField] private CalendarInput _calendarInput;
        [Space] [SerializeField] private Button _sendPinMessageButton;


        private DateTime _selectedDate;
        private string _writedMessage;
        public event UnityAction<PinMessageData> SendingDataConstructed;

        private bool _inWork;
        private bool _isSelectedDateValid;


        private void Awake()
        {
            _messageCalendar.DateSelected += OnDateSelected;
            _sendPinMessageButton.onClick.AddListener(SendPinMessageButton);
        }

        private void OnDestroy()
        {
            _messageCalendar.DateSelected -= OnDateSelected;
            _sendPinMessageButton.onClick.RemoveListener(SendPinMessageButton);
        }

        private void Update()
        {
            UpdateInteractableWithButton();
        }

        private void UpdateInteractableWithButton()
        {
            _sendPinMessageButton.interactable = IsDataValidToSendPinMessage();
        }

        private bool IsDataValidToSendPinMessage()
        {
            if (_isSelectedDateValid == false)
            {
                return false;
            }

            if (IsMessageExist(out _writedMessage) == false)
            {
                return false;
            }

            return true;
        }

        public void SendPinMessageButton()
        {
            if (_inWork)
                return;
            _inWork = true;

            if (IsDataValidToSendPinMessage() == false)
            {
                _inWork = false;
                return;
            }

            SendMessage();

            _inWork = false;
        }

        private async Task<bool> IsDateValid()
        {
            return _selectedDate != default && await IsDateAvailableForPin(_selectedDate);
        }

        private bool IsMessageExist(out string message)
        {
            message = _messageInputField.text;
            return string.IsNullOrWhiteSpace(_messageInputField.text) == false;
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

        private async void OnDateSelected(SelectedDateButtonData data)
        {
            _selectedDate = data.SelectedDate;
            _selectedDateTextsInCalendar.text = DateTools.ConvertToCustomFormat(_selectedDate);

            await UpdateValidData();
        }

        private async Task UpdateValidData()
        {
            _isSelectedDateValid = false;
            _isSelectedDateValid = await IsDateValid();
        }

        private void SendMessage()
        {
            PinMessageData pinMessageData = ConstructMessage();
            SendingDataConstructed?.Invoke(pinMessageData);
            DisableWindows();
            ResetCalendar();
        }

        private PinMessageData ConstructMessage()
        {
            PinMessageData pinMessageData = new()
                { Message = _chatDB.ConvertToChatMessage(_writedMessage), PinDate = _selectedDate };
            return pinMessageData;
        }

        private void ResetCalendar()
        {
            _selectedDateTextsInCalendar.text = "NOT SELECTED";
            _selectedDateTextsInMainWindow.text = "NOT SELECTED";
            _messageInputField.text = String.Empty;
            _selectedDate = default;
            _isSelectedDateValid = false;
            _calendarInput.ResetInput();
        }
    }
}