using System;
using System.Collections.Generic;
using System.Linq;
using ChatDB.PinMessage;
using MongoDBCustom;
using UnityEngine;
using Zenject;

public class CalendarDateCellMarker : MonoBehaviour
{
    private IDBCommands _idbCommands;
    [SerializeField] private Calendar _calendar;
    private List<PinMessageData> _pinMessages = new List<PinMessageData>();

    [Inject]
    public void Construct(IDBCommands idbCommands)
    {
        _idbCommands = idbCommands;
    }

    private void Awake()
    {
        _calendar.Updated += UpdateCalendar;
        UpdateDataFromDB();
    }

    private void OnDestroy()
    {
        _calendar.Updated -= UpdateCalendar;
    }

    public async void UpdateDataFromDB()
    {
        _pinMessages = await _idbCommands.GetAllPinnedMessageDatesAsync();
        UpdateCalendar();
    }

    private void UpdateCalendar()
    {
        DateTime[] busyDates = _pinMessages.Select(d => d.PinDate).ToArray();

        foreach (var button in _calendar.DayButtons)
        {
            button.image.color = Color.green;
            button.button.interactable = true;
        }

        foreach (var button in GetMatchButtons(busyDates))
        {
            button.image.color = Color.red;
            button.button.interactable = false;
        }

        foreach (var dayButton in GetMatchButtons(DateTime.Now))
        {
            dayButton.image.color = Color.yellow;
        }
    }

    private IEnumerable<DayButton> GetMatchButtons(params DateTime[] specialDates)
    {
        foreach (DayButton dayButton in _calendar.DayButtons)
        {
            if (dayButton.CurrentNumber() > 0 && dayButton.CurrentNumber() <= _calendar.DaysInMonth())
            {
                DateTime buttonDate = new DateTime(_calendar.SelectedYear, _calendar.SelectedMonth, dayButton.CurrentNumber());
                if (Array.Exists(specialDates, date => date.Date == buttonDate.Date))
                {
                    yield return dayButton;
                }
            }
        }
    }
}