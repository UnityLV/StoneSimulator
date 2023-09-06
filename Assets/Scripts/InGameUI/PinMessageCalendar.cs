using System;
using UnityEngine;
using UnityEngine.Events;
namespace ChatDB.PinMessage
{
    public class PinMessageCalendar : MonoBehaviour
    {
        [SerializeField] private CalendarInput _calendarInput;

        [SerializeField] private RectTransform _calendar;
        [SerializeField] private RectTransform _buttonsInCalendarUI;
        public event UnityAction<SelectedDateButtonData> DateSelected;
        

        private void OnEnable()
        {
            _calendarInput.DateSelected += OnDateSelected;
        }

        private void OnDisable()
        {
            _calendarInput.DateSelected -= OnDateSelected;
        }

        private void OnDateSelected(SelectedDateButtonData data)
        {
            DateSelected?.Invoke(data);
        }

        public void Show()
        {
            _calendar.gameObject.SetActive(true);
            _buttonsInCalendarUI.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _calendar.gameObject.SetActive(false);
            _buttonsInCalendarUI.gameObject.SetActive(false);
        }
    }
}