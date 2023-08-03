using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayButton : MonoBehaviour
{
    public Image image;
    public Button button;
    public Calendar calendar; //Calendar Script (attached to UI text for selected month
    public TMP_Text btext; //text of this button
    [Tooltip("The Original Number of This Button")]
    public int orgNumber; //original number of each button



    void Start()
    {
        NumberDays();
    }

    //Change the text in the button to the day of selected month
    //Blank out buttons that are not numbered
    public void NumberDays()
    {
        Image image = GetComponent<Image>();
        Button button = GetComponent<Button>();
        if (IsAvailableDay)
        {
            image.enabled = false;
            button.enabled = false;
            btext.text = "";
        }
        else
        {
            btext.text = CurrentNumber().ToString();
            button.enabled = true;
            image.enabled = true;
        }
    }

    private bool IsAvailableDay => CurrentNumber() <= 0 || CurrentNumber() > calendar.DaysInMonth();

    public int CurrentNumber()
    {
        return orgNumber - calendar.FirstOfMonthDay() + 1;
    }
}