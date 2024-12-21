using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDelivery_Server.Util
{
    static class Misc
    {
        public static string FormatDateTimeForText(DateTime dt)
        {
            var day = dt.Day.ToString();
            if (day.Length == 1) day = '0' + day;

            var month = dt.Month.ToString();
            if (month.Length == 1) month = '0' + month;

            var hour = dt.Hour.ToString();
            if (hour.Length == 1) hour = '0' + hour;

            var minute = dt.Minute.ToString();
            if (minute.Length == 1) minute = '0' + minute;

            return $"{day}.{month}.{dt.Year} {hour}:{minute}";
        }

        public static string FormatDateTimeForFilename(DateTime dt)
        {
            var day = dt.Day.ToString();
            if (day.Length == 1) day = '0' + day;

            var month = dt.Month.ToString();
            if (month.Length == 1) month = '0' + month;

            var hour = dt.Hour.ToString();
            if (hour.Length == 1) hour = '0' + hour;

            var minute = dt.Minute.ToString();
            if (minute.Length == 1) minute = '0' + minute;

            return $"{day}_{month}_{dt.Year}_{hour}_{minute}";
        }

        public static void Dump(string message)
        {
            Console.WriteLine($"{FormatDateTimeForText(DateTime.Now)} | {message}");
        }
    }
}
