using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CalendarInput : MonoBehaviour
{
    [SerializeField] private Calendar calendar;

    public event UnityAction<DateTime> DateSelected;

    private void Awake()
    {
        calendar = GameObject.FindObjectOfType<Calendar>();
    }

    public void UpdateField()
    {
        if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out DayButton dayButton))
        {
            int dayNumb = dayButton.CurrentNumber();

            DateSelected?.Invoke(calendar.ReturnDate(dayNumb));
        }
    }
}