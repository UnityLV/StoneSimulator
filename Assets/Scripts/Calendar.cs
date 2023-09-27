using UnityEngine;
using System;
using TMPro;
using UnityEngine.Events;


public class Calendar : MonoBehaviour
{
    private int selectedYear, selectedMonth, daysInMonth;
    private DateTime dtSelected;
    [SerializeField] private TMP_Text monthYearText;

    [SerializeField] private DayButton[] _dayButtons;

    public DayButton[] DayButtons => _dayButtons;

    public int SelectedYear => selectedYear;
    public int SelectedMonth => selectedMonth;
    public event UnityAction Updated;

    public void Awake()
    {
        dtSelected = DateTime.Today;
        selectedYear = dtSelected.Year;
        selectedMonth = dtSelected.Month;
        monthYearText.text = dtSelected.ToString("MMMM yyyy");
    }

    private void UpdateCalendar()
    {
        selectedYear = dtSelected.Year;
        selectedMonth = dtSelected.Month;
        monthYearText.text = dtSelected.ToString("MMMM yyyy");

        for (int i = 0; i < _dayButtons.Length; i++)
        {
            _dayButtons[i].NumberDays();
        }
        Updated?.Invoke();
    }

    public void SelectCurrentDate()
    {
        dtSelected = DateTime.Today;
        UpdateCalendar();
    }

    public void BackMonth()
    {
        dtSelected = dtSelected.AddMonths(-1);
        UpdateCalendar();
    }

    public void ForwardMonth()
    {
        dtSelected = dtSelected.AddMonths(1);
        UpdateCalendar();
    }

    public void BackYears()
    {
        dtSelected = dtSelected.AddYears(-1);
        UpdateCalendar();
    }

    public void ForwardYear()
    {
        dtSelected = dtSelected.AddYears(1);
        UpdateCalendar();
    }

    //Returns day of the week for the first day of the month. Sunday starts at 1 - Saturday is 7
    public int FirstOfMonthDay()
    {
        DateTime firstOfMonth = new DateTime(selectedYear, selectedMonth, 1);
        return (int)firstOfMonth.DayOfWeek + 1;
    }

    public int DaysInMonth()
    {
        return DateTime.DaysInMonth(selectedYear, selectedMonth);
    }

    public DateTime ReturnDate(int selectedDay)
    {
        DateTime selectedDate = new DateTime(selectedYear, selectedMonth, selectedDay);
        return selectedDate;
    }
}