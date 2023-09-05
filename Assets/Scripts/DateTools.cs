using System;

public static class DateTools
{
    public static DateTime[] GeneratePastDatesArray(int length)
    {
        DateTime[] pastDatesArray = new DateTime[length];

        DateTime currentDateTime = DateTime.Now;

        for (int i = 1; i < length; i++)
        {
            pastDatesArray[i] = currentDateTime.AddDays(-i);
        }

        return pastDatesArray;
    }
}