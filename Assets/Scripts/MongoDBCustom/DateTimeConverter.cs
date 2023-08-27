using System;
namespace MongoDBCustom
{
    public class DateTimeConverter
    {
        public static string ConvertToCustomFormat(DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy");
        }

        public static DateTime ConvertFromCustomFormat(string dateString)
        {
            return DateTime.ParseExact(dateString, "dd.MM.yyyy", null);
        }
    }
}