using System;
using System.Collections.Generic;
using System.Linq;
using ChatDB.PinMessage;
using MongoDBCustom;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;


public class CalendarDateCellMarker : MonoBehaviour
{
    private IDBCommands _idbCommands;
    [SerializeField] private Calendar _calendar;
    private List<PinMessageData> _pinMessages = new List<PinMessageData>();

    [SerializeField] private Sprite _unavailable;
    [SerializeField] private Sprite _available;
    [SerializeField] private Sprite _busy;
    private DateTime[] _elapsedDates;

    [Inject]
    public void Construct(IDBCommands idbCommands)
    {
        _idbCommands = idbCommands;
    }

    private void Awake()
    {
        _calendar.Updated += UpdateCalendarColors;
        UpdateDataFromDB();
        
        int elapsedDaysBuffer = 500;
        _elapsedDates = DateTools.GeneratePastDatesArray(elapsedDaysBuffer);
    }

    private void OnDestroy()
    {
        _calendar.Updated -= UpdateCalendarColors;
    }

    public async void UpdateDataFromDB()
    {
        _pinMessages = await _idbCommands.GetAllPinnedMessageDatesAsync();
        UpdateCalendarColors();
    }

    private void UpdateCalendarColors()
    {
        DateTime[] busyDates = _pinMessages.Select(d => d.PinDate).ToArray();

        SetAllAsAvailable();
        SetElapsedDates();
        SetBusyDates(busyDates);
    }

    private void SetAllAsAvailable()
    {
        foreach (DayButton button in _calendar.DayButtons)
        {
            button.SetImage(_available);
            button.button.interactable = true;
        }
    }

    private void SetBusyDates(DateTime[] busyDates)
    {
        foreach (var button in GetMatchButtons(busyDates))
        {
            button.SetImage(_busy);
            button.button.interactable = false;
        }
    }
    private void SetElapsedDates()
    {
        foreach (var button in GetMatchButtons(_elapsedDates))
        {
            button.SetImage(_unavailable);
            button.button.interactable = false;
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