using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectedDateButtonData
{
    public DayButton DayButton;
    public DateTime SelectedDate;
}

public class CalendarInput : MonoBehaviour
{
    [SerializeField] private Calendar calendar;
    public event UnityAction<SelectedDateButtonData> DateSelected;
    public SelectedDateButtonData SelectedButtonData { get; private set; } 
    private DayButton _selectedDayButton;

    private void Awake()
    {
        calendar = GameObject.FindObjectOfType<Calendar>();
    }

    public void ResetInput()
    {
        SelectedButtonData = null;
    }

    public void UpdateField()
    {
        if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out DayButton dayButton))
        {
            _selectedDayButton = dayButton;

            int dayNumb = _selectedDayButton.CurrentNumber();
            SelectedDateButtonData data = new SelectedDateButtonData { DayButton = _selectedDayButton, SelectedDate = calendar.ReturnDate(dayNumb) };
            SelectedButtonData = data;
            DateSelected?.Invoke(data);
        }
    }
}