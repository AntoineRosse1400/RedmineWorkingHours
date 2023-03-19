using System;
using System.Globalization;

namespace RedmineWorkingHours.ConsoleApp.Utils;

internal static class DateTimeUtils
{
    public static int GetWeekDays(DateTime begin, DateTime end)
    {
        return (int)(1 + ((end - begin).TotalDays * 5 -
                          (begin.DayOfWeek - end.DayOfWeek) * 2) / 7);
    }

    public static DateTime GetFirstDateOfMonth(int year, int month)
    {
        return new DateTime(year, month, 1);
    }

    public static DateTime GetLastDateOfMonth(int year, int month)
    {
        return new DateTime(year, month, DateTime.DaysInMonth(year, month));
    }
}