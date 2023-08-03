using System;
using System.Collections.Generic;
using UnityEngine;

public class ColoringCalendar : MonoBehaviour
{
    private Calendar _calendar;

    private void Awake()
    {
        _calendar = GameObject.FindObjectOfType<Calendar>();
        _calendar.Updated += UpdateCalendar;
    }

    private void OnDestroy()
    {
        _calendar.Updated -= UpdateCalendar;
    }


    private void UpdateCalendar()
    {
        // Create an array of special dates
        DateTime[] specialDates = new DateTime[] {
            new DateTime(2023, 8, 1),
            new DateTime(2023, 8, 2),
            new DateTime(2023, 8, 3),
            new DateTime(2023, 8, 31),
        };
        
        foreach (var button in _calendar.DayButtons)
        {
            button.image.color = Color.green;
            button.button.interactable = true;
        }

        foreach (var button in GetMatchButtons(specialDates))
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



    public void OnSpecialDateSelected(DayButton dayButton)
    {
        dayButton.image.color = Color.red;
    }
}