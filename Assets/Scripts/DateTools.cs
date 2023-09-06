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
    public static string ConvertToCustomFormat(DateTime dateTime)
    {
        return dateTime.ToString("dd.MM.yyyy");
    }

    public static DateTime ConvertFromCustomFormat(string dateString)
    {
        return DateTime.ParseExact(dateString, "dd.MM.yyyy", null);
    }
}