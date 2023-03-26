namespace RedmineWorkingHours.ConsoleApp.Utils;

internal static class DateTimeUtils
{
    public static int GetWeekDays(DateTime begin, DateTime end)
    {
        int days = (int)(end - begin).TotalDays + 1;
        int fullWeeks = days / 7;
        int weekdaysInFullWeeks = fullWeeks * 5;

        int extraDays = days % 7;
        int weekdaysInExtraDays = 0;

        if (extraDays <= 0)
            return weekdaysInFullWeeks;

        DateTime currentDate = begin.AddDays(fullWeeks * 7);
        while (extraDays > 0)
        {
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                weekdaysInExtraDays++;
            currentDate = currentDate.AddDays(1);
            extraDays--;
        }

        return weekdaysInFullWeeks + weekdaysInExtraDays;
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